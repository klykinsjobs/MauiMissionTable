using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class MissionServiceTests
    {
        [Fact]
        public async Task StartMissionAsync_SetsAssignedUnitsAndCompletionTime_AndThenSaves()
        {
            var state = new GameState { Missions = [] };
            var mission = new Mission { Duration = TimeSpan.FromSeconds(10) };
            state.Missions.Add(mission);
            var saver = new Mock<IGameStateSaver>();
            var assigned = new List<Unit> { new() { Id = "u1" } };
            var svc = new MissionService(state, saver.Object, new EventBus(), new SystemClock(), Mock.Of<IToastService>(),
                Mock.Of<IMissionGenerator>(), Mock.Of<IMissionExpirationService>(), Mock.Of<IMissionRefillService>(),
                Mock.Of<IMissionRewardService>(), Mock.Of<ISuccessChanceCalculator>());

            await svc.StartMissionAsync(mission, assigned, CancellationToken.None);

            Assert.NotNull(mission.CompletionTimeUtc);
            Assert.Equal([.. assigned.Select(u => u.Id)], mission.AssignedUnitIds);
            saver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ClaimMissionAsync_SuccessPathRemovesMission_AndThenSaves()
        {
            var clock = new SystemClock();
            var state = new GameState { Missions = [], Units = [] };
            var unit = new Unit { Id = "u1" };
            state.Units.Add(unit);
            var mission = new Mission { Duration = TimeSpan.FromSeconds(1), CompletionTimeUtc = clock.UtcNow.AddSeconds(-1) };
            state.Missions.Add(mission);
            mission.AssignedUnitIds.Add(unit.Id);
            
            var rewards = new Mock<IMissionRewardService>();
            var success = new Mock<ISuccessChanceCalculator>();
            var saver = new Mock<IGameStateSaver>();
            success.Setup(s => s.CalculateSuccessChance(It.IsAny<Mission>(), It.IsAny<IEnumerable<Unit>>())).Returns(100);
            rewards.Setup(r => r.RollSuccess(It.IsAny<int>())).Returns(true);
            rewards.Setup(r => r.ApplySuccessRewardsAsync(It.IsAny<Mission>(), It.IsAny<List<Unit>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var svc = new MissionService(state, saver.Object, new EventBus(), clock, Mock.Of<IToastService>(), Mock.Of<IMissionGenerator>(),
                Mock.Of<IMissionExpirationService>(), Mock.Of<IMissionRefillService>(), rewards.Object, success.Object);

            await svc.ClaimMissionAsync(mission, CancellationToken.None);

            Assert.DoesNotContain(mission, state.Missions);
            saver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ClaimMissionAsync_FailurePathRemovesMission_AndThenSaves()
        {
            var clock = new SystemClock();
            var state = new GameState { Missions = [], Units = [] };
            var unit = new Unit { Id = "u1" };
            state.Units.Add(unit);
            var mission = new Mission { Duration = TimeSpan.FromSeconds(1), CompletionTimeUtc = clock.UtcNow.AddSeconds(-1) };
            state.Missions.Add(mission);
            mission.AssignedUnitIds.Add(unit.Id);

            var rewards = new Mock<IMissionRewardService>();
            var success = new Mock<ISuccessChanceCalculator>();
            var saver = new Mock<IGameStateSaver>();
            success.Setup(s => s.CalculateSuccessChance(It.IsAny<Mission>(), It.IsAny<IEnumerable<Unit>>())).Returns(0);
            rewards.Setup(r => r.RollSuccess(It.IsAny<int>())).Returns(false);
            rewards.Setup(r => r.ApplyFailureRewardsAsync(It.IsAny<Mission>(), It.IsAny<List<Unit>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var svc = new MissionService(state, saver.Object, new EventBus(), clock, Mock.Of<IToastService>(), Mock.Of<IMissionGenerator>(),
                Mock.Of<IMissionExpirationService>(), Mock.Of<IMissionRefillService>(), rewards.Object, success.Object);

            await svc.ClaimMissionAsync(mission, CancellationToken.None);

            Assert.DoesNotContain(mission, state.Missions);
            saver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
