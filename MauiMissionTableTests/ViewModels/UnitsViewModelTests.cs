using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using Moq;

namespace MauiMissionTableTests.ViewModels
{
    public class UnitsViewModelTests
    {
        [Fact]
        public void Initializes_WithUnitRows()
        {
            var units = new List<Unit> { new() { Id = "u1", Title = "U1" }, new() { Id = "u2", Title = "U2" } };
            var state = new GameState { Units = units };
            var mockOrch = new Mock<IGameOrchestrator>();
            mockOrch.SetupGet(o => o.State).Returns(state);
            var gameState = new GameStateService(mockOrch.Object);

            var vm = new UnitsViewModel(gameState, new EventBus(), Mock.Of<INavigationService>());

            Assert.Equal(units.Count, vm.Units.Count);
            Assert.IsType<UnitRow>(vm.Units[0]);
            Assert.IsType<UnitRow>(vm.Units[1]);
        }
    }
}
