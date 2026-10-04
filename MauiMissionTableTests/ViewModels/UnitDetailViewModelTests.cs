using MauiMissionTable.Enums;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using Moq;

namespace MauiMissionTableTests.ViewModels
{
    public class UnitDetailViewModelTests
    {
        [Fact]
        public async Task SellCommand_CallsServiceAndNavigatesBack_OnSuccess()
        {
            var unit = new Unit { Id = "u1", Title = "U1", Rarity = Rarity.Common };
            var mockGameState = new Mock<IGameStateService>();
            var mockNav = new Mock<INavigationService>();
            var mockDialog = new Mock<IDialogService>();
            mockGameState.Setup(gs => gs.SellUnitAsync(It.IsAny<Unit>())).ReturnsAsync(true);   // succeed
            mockDialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);   // confirmation

            var vm = new UnitDetailViewModel(new EventBus(), mockGameState.Object, mockNav.Object,
                Mock.Of<IToastService>(), mockDialog.Object);
            vm.Initialize(unit);

            await vm.SellCommand.ExecuteAsync(null);

            mockGameState.Verify(g => g.SellUnitAsync(unit), Times.Once);
            mockNav.Verify(n => n.GoBackAsync(), Times.Once);
        }

        [Fact]
        public async Task SellCommand_ShowsToastAndDoesntNavigateAway_OnFailure()
        {
            var unit = new Unit { Id = "u1", Title = "U1", Rarity = Rarity.Common };
            var mockGameState = new Mock<IGameStateService>();
            var mockNav = new Mock<INavigationService>();
            var mockToast = new Mock<IToastService>();
            var mockDialog = new Mock<IDialogService>();
            mockGameState.Setup(gs => gs.SellUnitAsync(It.IsAny<Unit>())).ReturnsAsync(false);  // fail
            mockDialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);   // confirmation

            var vm = new UnitDetailViewModel(new EventBus(), mockGameState.Object, mockNav.Object, mockToast.Object,
                mockDialog.Object);
            vm.Initialize(unit);

            await vm.SellCommand.ExecuteAsync(null);

            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Cannot sell"))), Times.Once);
            mockNav.Verify(n => n.GoBackAsync(), Times.Never);
        }

        [Fact]
        public async Task UseXpBoostCommand_CallsService()
        {
            var unit = new Unit { Id = "u1", Title = "U1", Rarity = Rarity.Common, Level = 1, Xp = 0 };
            var mockGameState = new Mock<IGameStateService>();
            var mockDialog = new Mock<IDialogService>();
            mockGameState.Setup(gs => gs.UseXpBoostAsync(It.IsAny<Unit>())).ReturnsAsync(true); // succeed
            mockDialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);   // confirmation

            var vm = new UnitDetailViewModel(new EventBus(), mockGameState.Object, Mock.Of<INavigationService>(),
                Mock.Of<IToastService>(), mockDialog.Object);
            vm.Initialize(unit);

            await vm.UseXpBoostCommand.ExecuteAsync(null);

            mockGameState.Verify(g => g.UseXpBoostAsync(unit), Times.Once);
        }

        [Fact]
        public async Task UseXpBoostCommand_ShowsToastOnFailure()
        {
            var unit = new Unit { Id = "u1", Title = "U1", Rarity = Rarity.Common, Level = 1, Xp = 0 };
            var mockGameState = new Mock<IGameStateService>();
            var mockToast = new Mock<IToastService>();
            var mockDialog = new Mock<IDialogService>();
            mockGameState.Setup(gs => gs.UseXpBoostAsync(It.IsAny<Unit>())).ReturnsAsync(false);    // fail
            mockDialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);   // confirmation

            var vm = new UnitDetailViewModel(new EventBus(), mockGameState.Object, Mock.Of<INavigationService>(), mockToast.Object,
                mockDialog.Object);
            vm.Initialize(unit);

            await vm.UseXpBoostCommand.ExecuteAsync(null);

            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("No XP boosts available"))), Times.Once);
        }
    }
}
