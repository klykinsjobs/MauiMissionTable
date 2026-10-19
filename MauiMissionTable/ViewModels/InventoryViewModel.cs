using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;

namespace MauiMissionTable.ViewModels
{
    public partial class InventoryViewModel : BaseViewModel
    {
        private readonly IToastService _toastService;
        private readonly IEventBus _bus;
        private readonly IGameStateService _gameState;

        [ObservableProperty]
        public partial int Gold { get; set; }

        [ObservableProperty]
        public partial int Premium { get; set; }

        [ObservableProperty]
        public partial int RushBoosts { get; set; }

        [ObservableProperty]
        public partial int XpBoosts { get; set; }

        [ObservableProperty]
        public partial int PassiveGold { get; set; }

        [ObservableProperty]
        public partial int PassiveGoldMax { get; set; }

        [ObservableProperty]
        public partial int Packs { get; set; }

        private const int PassiveUpgradeCostPremium = 5;
        private const int PassiveUpgradeIncrement = 50;

        public InventoryViewModel(IGameStateService gameState, IEventBus bus, IToastService toastService)
        {
            _gameState = gameState;
            _bus = bus;
            _toastService = toastService;

            Title = "Inventory";

            _bus.Subscribe<GoldChangedEvent>(e => Gold = e.NewGold);
            _bus.Subscribe<PremiumChangedEvent>(e => Premium = e.NewPremium);
            _bus.Subscribe<RushBoostsChangedEvent>(e => RushBoosts = e.NewRushBoosts);
            _bus.Subscribe<XpBoostsChangedEvent>(e => XpBoosts = e.NewXpBoosts);
            _bus.Subscribe<PassiveGoldChangedEvent>(e => PassiveGold = e.NewPassiveGold);
            _bus.Subscribe<PassiveGoldMaxChangedEvent>(e => PassiveGoldMax = e.NewPassiveGoldMax);
            _bus.Subscribe<PacksChangedEvent>(e => Packs = e.NewPacks);

            Gold = _gameState.Gold;
            Premium = _gameState.Premium;
            RushBoosts = _gameState.RushBoosts;
            XpBoosts = _gameState.XpBoosts;
            PassiveGold = _gameState.PassiveGold;
            PassiveGoldMax = _gameState.PassiveGoldMax;
            Packs = _gameState.Packs;
        }

        [RelayCommand]
        private Task CollectAsync() => _gameState.CollectPassiveGoldAsync();

        [RelayCommand]
        private async Task UpgradePassiveAsync()
        {
            var success = await _gameState.UpgradePassiveCapacityAsync(PassiveUpgradeCostPremium, PassiveUpgradeIncrement);

            if (!success)
            {
                await _toastService.ShowAsync("Not enough premium currency to upgrade.");
                return;
            }
        }
    }
}
