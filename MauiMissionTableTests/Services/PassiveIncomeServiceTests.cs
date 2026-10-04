using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class PassiveIncomeServiceTests
    {
        [Fact]
        public void IncrementsPassiveGold_OnTick()
        {
            var bus = new EventBus();
            var clock = new SystemClock();
            var state = new GameState { PassiveGold = 0, PassiveGoldMax = 5 };
            _ = new PassiveIncomeService(state, new Mock<IGameStateSaver>().Object, bus, new Mock<IToastService>().Object);

            bus.Publish(new TickEvent(clock.UtcNow));

            Assert.Equal(1, state.PassiveGold);
        }

        [Fact]
        public async Task CollectPassiveGoldAsync_AddsPassiveGoldToGold_AndThenShowsToastAndSaves()
        {
            var state = new GameState { PassiveGold = 95, Gold = 5 };
            var mockSaver = new Mock<IGameStateSaver>();
            var mockToast = new Mock<IToastService>();
            var svc = new PassiveIncomeService(state, mockSaver.Object, new EventBus(), mockToast.Object);

            await svc.CollectPassiveGoldAsync(CancellationToken.None);

            Assert.Equal(100, state.Gold);
            Assert.Equal(0, state.PassiveGold);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Collected passive gold"))), Times.Once);
        }
    }
}
