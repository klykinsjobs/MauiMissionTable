using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class BoostServiceTests
    {
        [Fact]
        public async Task UseXpBoostAsync_DecrementsXpBoosts_AddsXpAndSaves()
        {
            var state = new GameState { XpBoosts = 1 };
            var unit = new Unit { Title = "U1", Xp = 0 };
            var mockUnitLevel = new Mock<IUnitLevelingService>();
            var mockToast = new Mock<IToastService>();
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new BoostService(state, mockSaver.Object, new EventBus(), new SystemClock(), mockToast.Object, mockUnitLevel.Object);

            bool result = await svc.UseXpBoostAsync(unit, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(0, state.XpBoosts);
            mockUnitLevel.Verify(u => u.AddXp(unit, 250), Times.Once);
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Used XP boost"))), Times.Once);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UseXpBoostAsync_ReturnsFalse_WhenNoBoosts()
        {
            var state = new GameState { XpBoosts = 0 };
            var unit = new Unit();
            var svc = new BoostService(state, Mock.Of<IGameStateSaver>(), new EventBus(), new SystemClock(), Mock.Of<IToastService>(),
                Mock.Of<IUnitLevelingService>());

            var result = await svc.UseXpBoostAsync(unit, CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task RushMissionAsync_DecrementsRushBoosts_ReducesCompletionTimeAndSaves()
        {
            var clock = new SystemClock();
            var state = new GameState { RushBoosts = 1 };
            var mission = new Mission { CompletionTimeUtc = clock.UtcNow.AddMinutes(10) };
            var mockToast = new Mock<IToastService>();
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new BoostService(state, mockSaver.Object, new EventBus(), clock, mockToast.Object, Mock.Of<IUnitLevelingService>());

            bool result = await svc.RushMissionAsync(mission, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(0, state.RushBoosts);
            Assert.True(mission.CompletionTimeUtc <= clock.UtcNow.AddMinutes(10));
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Mission rushed"))), Times.Once);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RushMissionAsync_ReturnsFalse_WhenNoBoosts()
        {
            var clock = new SystemClock();
            var state = new GameState { RushBoosts = 0 };
            var mission = new Mission { CompletionTimeUtc = clock.UtcNow.AddMinutes(10) };
            var svc = new BoostService(state, Mock.Of<IGameStateSaver>(), new EventBus(), clock, Mock.Of<IToastService>(),
                Mock.Of<IUnitLevelingService>());

            bool result = await svc.RushMissionAsync(mission, CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task BuyRushBoostAsync_IncrementsRushBoosts_DecrementsPremiumAndSaves()
        {
            var state = new GameState { Premium = 2, RushBoosts = 0 };
            var mockToast = new Mock<IToastService>();
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new BoostService(state, mockSaver.Object, new EventBus(), new SystemClock(), mockToast.Object,
                Mock.Of<IUnitLevelingService>());

            var result = await svc.BuyRushBoostAsync(2, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(0, state.Premium);
            Assert.Equal(1, state.RushBoosts);
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Bought 1 rush boost"))), Times.Once);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BuyRushBoostAsync_Fails_WhenNotEnoughPremium()
        {
            var state = new GameState { Premium = 0 };
            var svc = new BoostService(state, Mock.Of<IGameStateSaver>(), new EventBus(), new SystemClock(), Mock.Of<IToastService>(),
                Mock.Of<IUnitLevelingService>());

            var result = await svc.BuyRushBoostAsync(5, CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task BuyXpBoostAsync_IncrementsXpBoosts_DecrementsPremiumAndSaves()
        {
            var state = new GameState { Premium = 2, XpBoosts = 0 };
            var mockToast = new Mock<IToastService>();
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new BoostService(state, mockSaver.Object, new EventBus(), new SystemClock(), mockToast.Object,
                Mock.Of<IUnitLevelingService>());

            var result = await svc.BuyXpBoostAsync(2, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(0, state.Premium);
            Assert.Equal(1, state.XpBoosts);
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Bought 1 XP boost"))), Times.Once);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BuyXpBoostAsync_Fails_WhenNotEnoughPremium()
        {
            var state = new GameState { Premium = 0 };
            var svc = new BoostService(state, Mock.Of<IGameStateSaver>(), new EventBus(), new SystemClock(), Mock.Of<IToastService>(),
                Mock.Of<IUnitLevelingService>());

            bool result = await svc.BuyXpBoostAsync(2, CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task UpgradePassiveCapacityAsync_IncrementsPassiveGoldMax_DecrementsPremiumAndSaves()
        {
            var state = new GameState { Premium = 5, PassiveGoldMax = 100 };
            var mockToast = new Mock<IToastService>();
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new BoostService(state, mockSaver.Object, new EventBus(), new SystemClock(), mockToast.Object,
                Mock.Of<IUnitLevelingService>());

            var result = await svc.UpgradePassiveCapacityAsync(5, 50, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(0, state.Premium);
            Assert.Equal(150, state.PassiveGoldMax);
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Passive capacity increased"))), Times.Once);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpgradePassiveCapacityAsync_Fails_WhenNotEnoughPremium()
        {
            var state = new GameState { Premium = 0 };
            var svc = new BoostService(state, Mock.Of<IGameStateSaver>(), new EventBus(), new SystemClock(), Mock.Of<IToastService>(),
                Mock.Of<IUnitLevelingService>());

            var result = await svc.UpgradePassiveCapacityAsync(5, 50, CancellationToken.None);

            Assert.False(result);
        }
    }
}
