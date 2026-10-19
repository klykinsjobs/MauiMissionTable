using CommunityToolkit.Mvvm.Input;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Views;
using System.Collections.ObjectModel;

namespace MauiMissionTable.ViewModels
{
    public partial class UnitsViewModel : BaseViewModel
    {
        private readonly IEventBus _bus;
        private readonly IGameStateService _gameState;
        private readonly INavigationService _nav;

        public ObservableCollection<UnitRow> Units { get; } = [];

        public UnitsViewModel(IGameStateService gameState, IEventBus bus, INavigationService nav)
        {
            _gameState = gameState;
            _bus = bus;
            _nav = nav;

            Title = "Units";

            foreach (var unit in _gameState.Units)
                Units.Add(new UnitRow(unit));

            _bus.Subscribe<UnitAddedEvent>(OnUnitAdded);
            _bus.Subscribe<UnitRemovedEvent>(OnUnitRemoved);
            _bus.Subscribe<UnitUpdatedEvent>(OnUnitUpdated);
        }

        [RelayCommand]
        private async Task OpenUnitAsync(UnitRow row)
        {
            if (row?.Unit is null)
                return;

            await _nav.GoToAsync(nameof(UnitDetailPage), new { row.Unit });
        }

        private void OnUnitAdded(UnitAddedEvent unitAdded)
        {
            Units.Add(new UnitRow(unitAdded.Unit));
        }

        private void OnUnitRemoved(UnitRemovedEvent unitRemoved)
        {
            var existing = Units.FirstOrDefault(r => r.Unit.Id == unitRemoved.Unit.Id);
            if (existing != null)
                Units.Remove(existing);
        }

        private void OnUnitUpdated(UnitUpdatedEvent unitUpdated)
        {
            var existing = Units.FirstOrDefault(r => r.Unit.Id == unitUpdated.Unit.Id);
            existing?.Refresh();
        }
    }
}
