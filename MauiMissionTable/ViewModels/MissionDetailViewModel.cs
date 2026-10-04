using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using System.Collections.ObjectModel;

namespace MauiMissionTable.ViewModels
{
    [QueryProperty(nameof(Mission), "Mission")]
    public partial class MissionDetailViewModel : BaseViewModel
    {
        private readonly IEventBus _bus;
        private readonly IMissionService _missionService;
        private readonly IGameStateService _gameState;
        private readonly INavigationService _nav;
        private readonly IToastService _toastService;
        private readonly IDialogService _dialogService;
        private readonly IClock _clock;
        private bool _timerRunning;

        [ObservableProperty]
        public partial Mission Mission { get; set; } = new();

        public ObservableCollection<Unit> Units { get; } = [];          // All available units
        public ObservableCollection<Unit> AssignedUnits { get; } = [];  // Units assigned to this mission

        [ObservableProperty]
        public partial bool CanStartMission { get; set; }

        [ObservableProperty]
        public partial bool CanRushMission { get; set; }

        [ObservableProperty]
        public partial bool CanClaimMission { get; set; }

        [ObservableProperty]
        public partial double MissionProgress { get; set; }

        [ObservableProperty]
        public partial int SuccessChance { get; set; }

        public bool ShowUnitSelection => Mission.CompletionTimeUtc is null;

        public MissionDetailViewModel(IEventBus bus, IMissionService missionService, IGameStateService gameState,
            INavigationService nav, IToastService toastService, IDialogService dialogService, IClock clock)
        {
            _bus = bus;
            _missionService = missionService;
            _gameState = gameState;
            _nav = nav;
            _toastService = toastService;
            _dialogService = dialogService;
            _clock = clock;

            _bus.Subscribe<UnitAddedEvent>(OnUnitAdded);
            _bus.Subscribe<UnitRemovedEvent>(OnUnitRemoved);
            _bus.Subscribe<UnitUpdatedEvent>(OnUnitUpdated);
        }

        // Called automatically when Mission is set via QueryProperty
        partial void OnMissionChanged(Mission value)
        {
            Title = value.Title;

            Units.Clear();
            AssignedUnits.Clear();

            // Load available units
            foreach (var u in _gameState.Units)
            {
                if (!_missionService.IsUnitAssigned(u) || value.AssignedUnitIds.Contains(u.Id))
                    Units.Add(u);
            }

            // Load assigned units
            foreach (var id in value.AssignedUnitIds)
            {
                var unit = _gameState.Units.FirstOrDefault(u => u.Id == id);
                if (unit != null)
                    AssignedUnits.Add(unit);
            }

            UpdateState();
            EnsureTimer();
        }

        [RelayCommand]
        private void AssignUnit(Unit unit)
        {
            if (Mission.IsLocked)
                return;

            if (AssignedUnits.Contains(unit))
                return;

            if (AssignedUnits.Count >= 3)
                return;

            AssignedUnits.Add(unit);
            UpdateState();
        }

        [RelayCommand]
        private void RemoveUnit(Unit unit)
        {
            if (Mission.IsLocked)
                return;

            AssignedUnits.Remove(unit);
            UpdateState();
        }

        [RelayCommand]
        private void AutoAssign()
        {
            if (Mission.IsLocked)
                return;

            AssignedUnits.Clear();

            var best = _missionService.AutoAssignBestUnits(Mission, [.. Units]);
            foreach (var u in best.Take(3))
                AssignedUnits.Add(u);

            UpdateState();
        }
        
        [RelayCommand]
        private async Task StartMissionAsync()
        {
            if (!CanStartMission)
                return;

            await _missionService.StartMissionAsync(Mission, [.. AssignedUnits]);

            Mission.AssignedUnitIds = [.. AssignedUnits.Select(u => u.Id)];

            UpdateState();
            EnsureTimer();
        }

        [RelayCommand]
        private async Task ClaimAsync()
        {
            if (!CanClaimMission)
                return;

            await _missionService.ClaimMissionAsync(Mission);

            AssignedUnits.Clear();
            Mission.CompletionTimeUtc = null;

            UpdateState();
            await _nav.GoBackAsync();
        }

        [RelayCommand]
        private async Task RushAsync()
        {
            bool confirm = await _dialogService.ConfirmAsync("Rush Mission", "Spend a Rush Boost to reduce the time left for this mission?", "Rush", "Cancel");

            if (!confirm)
                return;

            if (!await _gameState.RushMissionAsync(Mission))
            {
                await _toastService.ShowAsync("Cannot rush this mission.");
                return;
            }

            UpdateState();
        }

        private void UpdateState()
        {
            OnPropertyChanged(nameof(ShowUnitSelection));
            
            SuccessChance = _missionService.CalculateSuccessChance(Mission, [.. AssignedUnits]);

            if (Mission.CompletionTimeUtc is null)
            {
                CanStartMission = AssignedUnits.Any();
                CanClaimMission = false;
                CanRushMission = false;
                MissionProgress = 0;
                return;
            }

            var now = _clock.UtcNow;
            var end = Mission.CompletionTimeUtc.Value;

            if (now < end)
            {
                CanStartMission = false;
                CanClaimMission = false;
                CanRushMission = true;

                var start = end - Mission.Duration;
                var total = Mission.Duration.TotalSeconds;

                if (total <= 0)
                {
                    MissionProgress = 1;
                    return;
                }

                var elapsed = (now - start).TotalSeconds;
                MissionProgress = Math.Clamp(elapsed / total, 0, 1);
            }
            else
            {
                CanStartMission = false;
                CanClaimMission = true;
                CanRushMission = false;
                MissionProgress = 1;
            }
        }

        private void EnsureTimer()
        {
            if (_timerRunning)
                return;

            _timerRunning = true;

            Application.Current?.Dispatcher.StartTimer(
                TimeSpan.FromSeconds(1),
                () =>
                {
                    if (Mission.CompletionTimeUtc is null)
                    {
                        _timerRunning = false;
                        return false;
                    }

                    UpdateState();
                    return Mission.CompletionTimeUtc > _clock.UtcNow;
                });
        }

        private void OnUnitAdded(UnitAddedEvent unitAdded)
        {
            if (!_missionService.IsUnitAssigned(unitAdded.Unit))
                Units.Add(unitAdded.Unit);
        }

        private void OnUnitRemoved(UnitRemovedEvent unitRemoved)
        {
            Units.Remove(unitRemoved.Unit);
            AssignedUnits.Remove(unitRemoved.Unit);
            UpdateState();
        }

        private void OnUnitUpdated(UnitUpdatedEvent unitUpdated)
        {
            // Update available
            var existing = Units.FirstOrDefault(u => u.Id == unitUpdated.Unit.Id);
            if (existing != null)
            {
                int index = Units.IndexOf(existing);
                Units[index] = unitUpdated.Unit;
            }

            // Update assigned
            var assigned = AssignedUnits.FirstOrDefault(u => u.Id == unitUpdated.Unit.Id);
            if (assigned != null)
            {
                int index = AssignedUnits.IndexOf(assigned);
                AssignedUnits[index] = unitUpdated.Unit;
            }

            UpdateState();
        }
    }
}
