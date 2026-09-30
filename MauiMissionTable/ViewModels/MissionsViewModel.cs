using CommunityToolkit.Mvvm.Input;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Views;
using System.Collections.ObjectModel;

namespace MauiMissionTable.ViewModels
{
    public partial class MissionsViewModel : BaseViewModel
    {
        private readonly IEventBus _bus;
        private readonly IMissionService _missionService;
        private readonly INavigationService _nav;
        private readonly IClock _clock;
        private bool _timerRunning;

        public ObservableCollection<MissionRow> Missions { get; } = [];

        public MissionsViewModel(IEventBus bus, IMissionService missionService, INavigationService nav, IClock clock)
        {
            _bus = bus;
            _missionService = missionService;
            _nav = nav;
            _clock = clock;

            Title = "Missions";

            foreach (var m in _missionService.Missions)
                Missions.Add(new MissionRow(m, _clock));

            _bus.Subscribe<MissionUpdatedEvent>(OnMissionUpdated);
            _bus.Subscribe<MissionsChangedEvent>(OnMissionsChanged);

            StartTimer();
        }

        private void StartTimer()
        {
            if (_timerRunning)
                return;

            _timerRunning = true;

            Application.Current?.Dispatcher.StartTimer(
                TimeSpan.FromSeconds(1),
                () =>
                {
                    if (!_timerRunning)
                        return false;

                    foreach (var row in Missions)
                        row.Refresh();

                    return true;
                });
        }

        public void StopTimer()
        {
            _timerRunning = false;
        }

        [RelayCommand]
        private async Task OpenMissionAsync(MissionRow row)
        {
            await _nav.GoToAsync(nameof(MissionDetailPage), new { row.Mission });
        }

        private void OnMissionUpdated(MissionUpdatedEvent missionUpdated)
        {
            var row = Missions.FirstOrDefault(r => r.Mission.Id == missionUpdated.Mission.Id);
            row?.Refresh();
        }

        private void OnMissionsChanged(MissionsChangedEvent missionsChanged)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Missions.Clear();
                foreach (var m in _missionService.Missions)
                    Missions.Add(new MissionRow(m, _clock));
            });
        }
    }
}
