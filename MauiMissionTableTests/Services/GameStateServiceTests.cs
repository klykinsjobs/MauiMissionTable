using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class GameStateServiceTests
    {
        [Fact]
        public void Properties_ReflectOrchestratorState()
        {
            var state = new GameState
            {
                Gold = 10,
                PassiveGold = 2,
                PassiveGoldMax = 100,
                Premium = 3,
                RushBoosts = 1,
                XpBoosts = 4,
                Packs = 2,
                Units = [new Unit { Id = "u1" }]
            };

            var mockOrch = new Mock<IGameOrchestrator>();
            mockOrch.SetupGet(o => o.State).Returns(state);

            var svc = new GameStateService(mockOrch.Object);

            Assert.Equal(10, svc.Gold);
            Assert.Equal(2, svc.PassiveGold);
            Assert.Equal(100, svc.PassiveGoldMax);
            Assert.Equal(3, svc.Premium);
            Assert.Equal(1, svc.RushBoosts);
            Assert.Equal(4, svc.XpBoosts);
            Assert.Equal(2, svc.Packs);
            Assert.Single(svc.Units);
        }
    }
}
