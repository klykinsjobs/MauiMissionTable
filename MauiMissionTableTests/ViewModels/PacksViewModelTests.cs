using MauiMissionTable.Enums;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using Moq;

namespace MauiMissionTableTests.ViewModels
{
    public class PacksViewModelTests
    {
        [Fact]
        public async Task OpenPackCommand_UpdatesStatusAndRewards()
        {
            var mockGameState = new Mock<IGameStateService>();
            var rewards = new List<PackReward> { new() { RewardType = PackRewardType.Gold, Amount = 10, Rarity = Rarity.Common } };
            mockGameState.Setup(g => g.OpenPackAsync()).ReturnsAsync(rewards);
            mockGameState.SetupGet(g => g.Packs).Returns(1);
            var vm = new PacksViewModel(mockGameState.Object, new EventBus());

            await vm.OpenPackCommand.ExecuteAsync(null);

            Assert.Contains("You received", vm.Status);
            Assert.Single(vm.LatestRewards);
        }

        [Fact]
        public void Reset_ClearsRewardsAndStatus()
        {
            var mock = new Mock<IGameStateService>();
            var vm = new PacksViewModel(mock.Object, new EventBus());
            vm.LatestRewards.Add(new PackReward { RewardType = PackRewardType.Gold, Amount = 10, Rarity = Rarity.Common } );
            vm.Status = "Lorem ipsum";

            vm.Reset();

            Assert.Empty(vm.LatestRewards);
            Assert.Equal("Tap to open a pack", vm.Status);
        }
    }
}
