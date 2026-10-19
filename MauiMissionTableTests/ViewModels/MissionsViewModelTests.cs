using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using Moq;

namespace MauiMissionTableTests.ViewModels
{
    public class MissionsViewModelTests
    {
        [Fact]
        public void Initializes_WithMissionRows()
        {
            var missions = new List<Mission> { new() { Title = "A" }, new() { Title = "B" } };
            var state = new GameState { Missions = missions };
            var missionService = new Mock<IMissionService>();
            missionService.SetupGet(m => m.Missions).Returns(state.Missions);

            var vm = new MissionsViewModel(new EventBus(), missionService.Object, Mock.Of<INavigationService>(), new SystemClock());

            Assert.Equal(missions.Count, vm.Missions.Count);
            Assert.IsType<MissionRow>(vm.Missions[0]);
            Assert.IsType<MissionRow>(vm.Missions[1]);
        }
    }
}
