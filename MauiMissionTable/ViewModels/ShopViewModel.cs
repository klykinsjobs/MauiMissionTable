using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;

namespace MauiMissionTable.ViewModels
{
    public partial class ShopViewModel : BaseViewModel
    {
        private readonly IEventBus _bus;
        private readonly IGameStateService _gameState;
        private readonly IToastService _toastService;

        [ObservableProperty]
        public partial int Gold { get; set; }

        [ObservableProperty]
        public partial int Premium { get; set; }

        private const int PackGoldCost = 50;
        private const int RushBoostPremiumCost = 2;
        private const int XpBoostPremiumCost = 2;

        public ShopViewModel(IGameStateService gameState, IEventBus bus, IToastService toastService)
        {
            _gameState = gameState;
            _bus = bus;
            _toastService = toastService;

            Title = "Shop";

            _bus.Subscribe<GoldChangedEvent>(e => Gold = e.NewGold);
            _bus.Subscribe<PremiumChangedEvent>(e => Premium = e.NewPremium);

            Gold = _gameState.Gold;
            Premium = _gameState.Premium;
        }

        [RelayCommand]
        private async Task BuyPackAsync()
        {
            if (!await _gameState.BuyPackWithGoldAsync(PackGoldCost))
            {
                await _toastService.ShowAsync("Not enough gold for a pack.");
                return;
            }
        }

        [RelayCommand]
        private async Task BuyRushBoostAsync()
        {
            if (!await _gameState.BuyRushBoostAsync(RushBoostPremiumCost))
            {
                await _toastService.ShowAsync("Not enough premium currency.");
                return;
            }
        }

        [RelayCommand]
        private async Task BuyXpBoostAsync()
        {
            if (!await _gameState.BuyXpBoostAsync(XpBoostPremiumCost))
            {
                await _toastService.ShowAsync("Not enough premium currency.");
                return;
            }
        }
    }
}
