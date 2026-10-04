using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using Moq;

namespace MauiMissionTableTests.ViewModels
{
    public class MissionDetailViewModelTests
    {
        [Fact]
        public async Task StartMissionCommand_CallsService()
        {
            var mission = new Mission { Duration = TimeSpan.FromSeconds(1) };
            var u1 = new Unit { Id = "u1" };
            var missionService = new Mock<IMissionService>();
            var gameState = new Mock<IGameStateService>();
            missionService.Setup(m => m.StartMissionAsync(It.IsAny<Mission>(), It.IsAny<IEnumerable<Unit>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            gameState.SetupGet(g => g.Units).Returns([u1]);

            var vm = new MissionDetailViewModel(new EventBus(), missionService.Object, gameState.Object, Mock.Of<INavigationService>(),
                Mock.Of<IToastService>(), Mock.Of<IDialogService>(), new SystemClock())
            {
                Mission = mission
            };
            vm.AssignedUnits.Add(u1);
            vm.CanStartMission = true;

            await vm.StartMissionCommand.ExecuteAsync(null);

            Assert.Contains(u1.Id, vm.Mission.AssignedUnitIds);
            missionService.Verify(m => m.StartMissionAsync(mission, vm.AssignedUnits, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ClaimCommand_CallsServiceAndNavigatesBack_OnSuccess()
        {
            var clock = new SystemClock();
            var mission = new Mission { CreatedUtc = clock.UtcNow.AddHours(-5), Duration = TimeSpan.FromHours(1),
                CompletionTimeUtc = clock.UtcNow };
            var u1 = new Unit { Id = "u1" };
            mission.AssignedUnitIds.Add(u1.Id);
            var missionService = new Mock<IMissionService>();
            var gameState = new Mock<IGameStateService>();
            var nav = new Mock<INavigationService>();
            gameState.SetupGet(g => g.Units).Returns([u1]);
            missionService.Setup(m => m.ClaimMissionAsync(It.IsAny<Mission>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var vm = new MissionDetailViewModel(new EventBus(), missionService.Object, gameState.Object, nav.Object, Mock.Of<IToastService>(),
                Mock.Of<IDialogService>(), clock)
            {
                Mission = mission
            };

            await vm.ClaimCommand.ExecuteAsync(null);

            Assert.Null(mission.CompletionTimeUtc);
            Assert.Empty(vm.AssignedUnits);
            missionService.Verify(m => m.ClaimMissionAsync(mission, It.IsAny<CancellationToken>()), Times.Once);
            nav.Verify(n => n.GoBackAsync(), Times.Once);
        }

        [Fact]
        public async Task RushCommand_CallsService_OnSuccess()
        {
            var clock = new SystemClock();
            var mission = new Mission { CreatedUtc = clock.UtcNow, Duration = TimeSpan.FromMinutes(5),
                CompletionTimeUtc = clock.UtcNow.AddMinutes(5) };
            var u1 = new Unit { Id = "u1" };
            mission.AssignedUnitIds.Add(u1.Id);
            var missionService = new Mock<IMissionService>();
            var gameState = new Mock<IGameStateService>();
            var dialog = new Mock<IDialogService>();
            gameState.SetupGet(g => g.Units).Returns([u1]);
            gameState.Setup(g => g.RushMissionAsync(It.IsAny<Mission>())).ReturnsAsync(true);   // succeed
            dialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);   // confirmation

            var vm = new MissionDetailViewModel(new EventBus(), missionService.Object, gameState.Object, Mock.Of<INavigationService>(),
                Mock.Of<IToastService>(), dialog.Object, clock)
            {
                Mission = mission
            };

            await vm.RushCommand.ExecuteAsync(null);

            Assert.True(mission.CompletionTimeUtc <= clock.UtcNow.AddMinutes(5));
            gameState.Verify(g => g.RushMissionAsync(mission), Times.Once);
        }

        [Fact]
        public async Task RushCommand_ShowsToast_OnFailure()
        {
            var clock = new SystemClock();
            var mission = new Mission { CreatedUtc = clock.UtcNow, Duration = TimeSpan.FromMinutes(5),
                CompletionTimeUtc = clock.UtcNow.AddMinutes(5) };
            var u1 = new Unit { Id = "u1" };
            mission.AssignedUnitIds.Add(u1.Id);
            var gameState = new Mock<IGameStateService>();
            var toast = new Mock<IToastService>();
            var dialog = new Mock<IDialogService>();
            gameState.SetupGet(g => g.Units).Returns([u1]);
            gameState.Setup(g => g.RushMissionAsync(It.IsAny<Mission>())).ReturnsAsync(false);  // fail
            dialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);   // confirmation

            var vm = new MissionDetailViewModel(new EventBus(), Mock.Of<IMissionService>(), gameState.Object, Mock.Of<INavigationService>(),
                toast.Object, dialog.Object, clock)
            {
                Mission = mission
            };

            await vm.RushCommand.ExecuteAsync(null);

            toast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Cannot rush"))), Times.Once);
        }
    }
}
