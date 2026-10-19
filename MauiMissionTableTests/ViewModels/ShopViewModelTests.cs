using MauiMissionTable.Interfaces;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using Moq;

namespace MauiMissionTableTests.ViewModels
{
    public class ShopViewModelTests
    {
        [Fact]
        public async Task BuyPackCommand_CallsService()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.BuyPackWithGoldAsync(It.IsAny<int>())).ReturnsAsync(true);
            var vm = new ShopViewModel(gameState.Object, new EventBus(), Mock.Of<IToastService>());

            await vm.BuyPackCommand.ExecuteAsync(null);

            gameState.Verify(g => g.BuyPackWithGoldAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task BuyPackCommand_ShowsToastOnFailure()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.BuyPackWithGoldAsync(It.IsAny<int>())).ReturnsAsync(false);
            var toast = new Mock<IToastService>();
            var vm = new ShopViewModel(gameState.Object, new EventBus(), toast.Object);

            await vm.BuyPackCommand.ExecuteAsync(null);

            toast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Not enough gold"))), Times.Once);
        }

        [Fact]
        public async Task BuyRushBoostCommand_CallsService()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.BuyRushBoostAsync(It.IsAny<int>())).ReturnsAsync(true);
            var vm = new ShopViewModel(gameState.Object, new EventBus(), Mock.Of<IToastService>());

            await vm.BuyRushBoostCommand.ExecuteAsync(null);

            gameState.Verify(g => g.BuyRushBoostAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task BuyRushBoostCommand_ShowsToastOnFailure()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.BuyRushBoostAsync(It.IsAny<int>())).ReturnsAsync(false);
            var toast = new Mock<IToastService>();
            var vm = new ShopViewModel(gameState.Object, new EventBus(), toast.Object);

            await vm.BuyRushBoostCommand.ExecuteAsync(null);

            toast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Not enough premium"))), Times.Once);
        }

        [Fact]
        public async Task BuyXpBoostCommand_CallsService()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.BuyXpBoostAsync(It.IsAny<int>())).ReturnsAsync(true);
            var vm = new ShopViewModel(gameState.Object, new EventBus(), Mock.Of<IToastService>());

            await vm.BuyXpBoostCommand.ExecuteAsync(null);

            gameState.Verify(g => g.BuyXpBoostAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task BuyXpBoostCommand_ShowsToastOnFailure()
        {
            var gameState = new Mock<IGameStateService>();
            gameState.Setup(g => g.BuyXpBoostAsync(It.IsAny<int>())).ReturnsAsync(false);
            var toast = new Mock<IToastService>();
            var vm = new ShopViewModel(gameState.Object, new EventBus(), toast.Object);

            await vm.BuyXpBoostCommand.ExecuteAsync(null);

            toast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Not enough premium"))), Times.Once);
        }
    }
}
