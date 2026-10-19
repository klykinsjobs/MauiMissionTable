using MauiMissionTable.Interfaces;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using Moq;

namespace MauiMissionTableTests.ViewModels
{
    public class InventoryViewModelTests
    {
        [Fact]
        public async Task CollectCommand_CallsService()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.CollectPassiveGoldAsync()).Returns(Task.CompletedTask);
            var vm = new InventoryViewModel(gameState.Object, new EventBus(), Mock.Of<IToastService>());

            await vm.CollectCommand.ExecuteAsync(null);

            gameState.Verify(g => g.CollectPassiveGoldAsync(), Times.Once);
        }

        [Fact]
        public async Task UpgradePassiveCommand_CallsService()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.UpgradePassiveCapacityAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var vm = new InventoryViewModel(gameState.Object, new EventBus(), Mock.Of<IToastService>());

            await vm.UpgradePassiveCommand.ExecuteAsync(null);

            gameState.Verify(g => g.UpgradePassiveCapacityAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpgradePassiveCommand_ShowsToastOnFailure()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.UpgradePassiveCapacityAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            var toast = new Mock<IToastService>();
            var vm = new InventoryViewModel(gameState.Object, new EventBus(), toast.Object);

            await vm.UpgradePassiveCommand.ExecuteAsync(null);

            toast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Not enough premium"))), Times.Once);
        }
    }
}
