using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.ViewModels
{
    public partial class UnitDetailViewModel : BaseViewModel
    {
        private readonly IEventBus _bus;
        private readonly IGameStateService _gameState;
        private readonly INavigationService _nav;
        private readonly IToastService _toastService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        public partial Unit Unit { get; set; } = new();

        [ObservableProperty]
        public partial int XpToNextLevel { get; set; }

        [ObservableProperty]
        public partial double XpProgress { get; set; }

        public UnitDetailViewModel(IEventBus bus, IGameStateService gameState, INavigationService nav,
            IToastService toastService, IDialogService dialogService)
        {
            _bus = bus;
            _gameState = gameState;
            _nav = nav;
            _toastService = toastService;
            _dialogService = dialogService;

            _bus.Subscribe<UnitUpdatedEvent>(OnUnitUpdated);
        }

        public void Initialize(Unit unit)
        {
            Unit = unit;
            XpToNextLevel = unit.XpToNextLevel;
            XpProgress = unit.XpProgress;
            Title = unit.Title;
        }

        [RelayCommand]
        private async Task SellAsync()
        {
            bool confirm = await _dialogService.ConfirmAsync("Sell Unit", $"Sell {Unit.Title} for gold?", "Sell", "Cancel");

            if (!confirm)
                return;

            if (!await _gameState.SellUnitAsync(Unit))
            {
                await _toastService.ShowAsync("Cannot sell this unit.");
                return;
            }

            await _nav.GoBackAsync();
        }

        [RelayCommand]
        private async Task UseXpBoostAsync()
        {
            bool confirm = await _dialogService.ConfirmAsync("Use XP Boost", $"Use an XP boost on {Unit.Title}?", "Use", "Cancel");

            if (!confirm)
                return;

            if (!await _gameState.UseXpBoostAsync(Unit))
            {
                await _toastService.ShowAsync("No XP boosts available or unit is on a mission.");
                return;
            }
        }

        private void OnUnitUpdated(UnitUpdatedEvent unitUpdated)
        {
            if (unitUpdated.Unit.Id == Unit.Id)
                Initialize(unitUpdated.Unit);
        }
    }
}
