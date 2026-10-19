using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class DailyRewardServiceTests
    {
        [Fact]
        public void AddsPack_WhenNotClaimedToday()
        {
            var bus = new EventBus();
            var clock = new SystemClock();
            var state = new GameState { Packs = 0, LastDailyPackUtc = null };
            var mockSaver = new Mock<IGameStateSaver>();
            var svc = new DailyRewardService(state, mockSaver.Object, bus, clock, new Mock<IToastService>().Object);

            bus.Publish(new TickEvent(clock.UtcNow));

            Assert.Equal(1, state.Packs);
            Assert.NotNull(state.LastDailyPackUtc);
            mockSaver.Verify(s => s.SaveAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public void DoesNotAddPack_IfAlreadyGivenToday()
        {
            var bus = new EventBus();
            var clock = new SystemClock();
            var state = new GameState { Packs = 1, LastDailyPackUtc = clock.UtcNow };
            _ = new DailyRewardService(state, Mock.Of<IGameStateSaver>(), bus, clock, new Mock<IToastService>().Object);

            bus.Publish(new TickEvent(clock.UtcNow));

            Assert.Equal(1, state.Packs);
        }
    }
}
