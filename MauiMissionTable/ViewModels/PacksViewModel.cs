using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using System.Collections.ObjectModel;

namespace MauiMissionTable.ViewModels
{
    public partial class PacksViewModel : BaseViewModel
    {
        private readonly IGameStateService _gameState;
        private readonly IEventBus _bus;

        public ObservableCollection<PackReward> LatestRewards { get; } = [];

        [ObservableProperty]
        public partial int Packs { get; set; }

        [ObservableProperty]
        public partial string Status { get; set; } = "Tap to open a pack";

        public PacksViewModel(IGameStateService gameState, IEventBus bus)
        {
            _gameState = gameState;
            _bus = bus;

            Title = "Packs";

            _bus.Subscribe<PacksChangedEvent>(e => Packs = e.NewPacks);

            Packs = _gameState.Packs;
        }

        [RelayCommand]
        private async Task OpenPackAsync()
        {
            if (Packs <= 0)
            {
                Status = "No packs available!";
                return;
            }

            Status = "Opening pack...";
            await Task.Delay(600);

            LatestRewards.Clear();

            var rewards = await _gameState.OpenPackAsync();

            foreach (var reward in rewards)
                LatestRewards.Add(reward);

            Status = $"You received {rewards.Count} rewards!";
        }

        public void Reset()
        {
            LatestRewards.Clear();
            Status = "Tap to open a pack";
        }
    }
}
