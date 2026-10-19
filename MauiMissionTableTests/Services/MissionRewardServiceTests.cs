using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class MissionRewardServiceTests
    {
        [Fact]
        public async Task ApplySuccessRewardsAsync_GivesRewardsAndShowsToast()
        {
            var state = new GameState { Gold = 0, RushBoosts = 0, XpBoosts = 0, Premium = 0 };
            var mission = new Mission { GoldReward = 10, XpReward = 123, RushBoosts = 10, XpBoosts = 10, PremiumReward = 10 };
            var units = new List<Unit> { new() { Id = "u1" } };
            var mockLevel = new Mock<IUnitLevelingService>();
            var mockToast = new Mock<IToastService>();
            var svc = new MissionRewardService(state, new EventBus(), new RandomProvider(), mockToast.Object, mockLevel.Object);

            await svc.ApplySuccessRewardsAsync(mission, units, CancellationToken.None);

            Assert.Equal(10, state.Gold);
            Assert.Equal(10, state.RushBoosts);
            Assert.Equal(10, state.XpBoosts);
            Assert.Equal(10, state.Premium);
            mockLevel.Verify(l => l.AddXp(It.IsAny<Unit>(), mission.XpReward), Times.Exactly(units.Count));
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Success"))), Times.Once);
        }

        [Fact]
        public async Task ApplyFailureRewardsAsync_GivesPartialXpAndShowsToast()
        {
            var mission = new Mission { XpReward = 123 };
            var partialXp = (int)(mission.XpReward * 0.1);
            var units = new List<Unit> { new() { Id = "u1" } };
            var mockLevel = new Mock<IUnitLevelingService>();
            var mockToast = new Mock<IToastService>();
            var svc = new MissionRewardService(new GameState(), new EventBus(), new RandomProvider(), mockToast.Object, mockLevel.Object);

            await svc.ApplyFailureRewardsAsync(mission, units, CancellationToken.None);

            mockLevel.Verify(l => l.AddXp(It.IsAny<Unit>(), partialXp), Times.Exactly(units.Count));
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Mission failed"))), Times.Once);
        }
    }
}
