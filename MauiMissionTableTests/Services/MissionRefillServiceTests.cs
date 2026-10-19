using MauiMissionTable.Models;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class MissionRefillServiceTests
    {
        [Fact]
        public void TryRefillMissions_AddsUntilMax()
        {
            var clock = new SystemClock();
            var state = new GameState { Missions = [], MaxMissions = 3 };
            var generator = new MissionGenerator(state, clock, new RandomProvider());
            var refill = new MissionRefillService(state, clock, TimeSpan.FromSeconds(0));

            bool result = refill.TryRefillMissions(generator, out int added);

            Assert.True(result);
            Assert.Equal(3, state.Missions.Count);
            Assert.Equal(3, added);
        }

        [Fact]
        public void TryRefillMissions_DoesNotAdd_WhenAtMaxMissions()
        {
            var clock = new SystemClock();
            var state = new GameState { Missions = [], MaxMissions = 2 };
            var generator = new MissionGenerator(state, clock, new RandomProvider());

            // Fill to max
            state.Missions.Add(new Mission());
            state.Missions.Add(new Mission());

            var refill = new MissionRefillService(state, clock, TimeSpan.FromSeconds(0));

            bool result = refill.TryRefillMissions(generator, out int added);

            Assert.False(result);
            Assert.Equal(0, added);
        }

        [Fact]
        public async Task TryRefillMissions_DoesNotAdd_WhenIntervalHasntElapsed()
        {
            var clock = new SystemClock();
            var state = new GameState { Missions = [], MaxMissions = 3 };
            var generator = new MissionGenerator(state, clock, new RandomProvider());
            var refill = new MissionRefillService(state, clock, TimeSpan.FromSeconds(10));

            // First refill should succeed
            bool first = refill.TryRefillMissions(generator, out int addedFirst);
            Assert.True(first);
            Assert.True(addedFirst > 0);

            // Clear all missions so there is room to refill
            state.Missions.Clear();

            // Immediately try again - should fail due to interval
            bool second = refill.TryRefillMissions(generator, out int addedSecond);
            Assert.False(second);
            Assert.Equal(0, addedSecond);
        }
    }
}
