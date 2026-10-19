using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class GameOrchestratorTests
    {
        [Fact]
        public async Task InitializeAsync_LoadsStateAndStartsLoop()
        {
            var defaultState = GameStateFactory.CreateDefault();
            defaultState.Gold = 42;
            var mockRepo = new Mock<IGameRepository>();
            mockRepo.Setup(r => r.LoadAsync(It.IsAny<CancellationToken>())).ReturnsAsync(defaultState);
            var state = new GameState();
            var loop = new Mock<IGameLoopService>();
            var orchestrator = new GameOrchestrator(state, mockRepo.Object, loop.Object, Mock.Of<IMissionService>(),
                Mock.Of<IUnitService>(), Mock.Of<IEconomyService>(), Mock.Of<IDailyRewardService>());

            await orchestrator.InitializeAsync();

            Assert.Equal(42, state.Gold);
            loop.Verify(l => l.Start(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DisposeAsync_DisposesLoop()
        {
            var loop = new Mock<IGameLoopService>();
            var orchestrator = new GameOrchestrator(new GameState(), Mock.Of<IGameRepository>(), loop.Object,
                Mock.Of<IMissionService>(), Mock.Of<IUnitService>(), Mock.Of<IEconomyService>(), Mock.Of<IDailyRewardService>());

            await orchestrator.DisposeAsync();

            loop.Verify(l => l.DisposeAsync(), Times.Once);
        }
    }
}
