using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class EconomyServiceTests
    {
        [Fact]
        public async Task AddGoldAsync_IncrementsGoldAndSaves()
        {
            var state = new GameState { Gold = 0 };
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new EconomyService(state, mockSaver.Object, new EventBus(), Mock.Of<IBoostService>(),
                Mock.Of<IPackOpeningService>(), Mock.Of<IPassiveIncomeService>());

            await svc.AddGoldAsync(123, CancellationToken.None);
            
            Assert.Equal(123, state.Gold);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SpendGoldAsync_DecrementsGoldAndSaves()
        {
            var state = new GameState { Gold = 123 };
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new EconomyService(state, mockSaver.Object, new EventBus(), Mock.Of<IBoostService>(),
                Mock.Of<IPackOpeningService>(), Mock.Of<IPassiveIncomeService>());

            var result = await svc.SpendGoldAsync(123, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(0, state.Gold);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SpendGoldAsync_Fails_WhenNotEnoughGold()
        {
            var state = new GameState { Gold = 0 };
            var svc = new EconomyService(state, Mock.Of<IGameStateSaver>(), new EventBus(), Mock.Of<IBoostService>(),
                Mock.Of<IPackOpeningService>(), Mock.Of<IPassiveIncomeService>());

            var result = await svc.SpendGoldAsync(123, CancellationToken.None);

            Assert.False(result);
        }
    }
}
