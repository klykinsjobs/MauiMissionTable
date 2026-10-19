using MauiMissionTable.Models;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class MissionExpirationServiceTests
    {
        [Fact]
        public void RemoveExpiredMissions_RemovesOnlyExpiredAndUnlockedMissions()
        {
            var now = DateTime.UtcNow;
            var state = new GameState
            {
                Missions =
                [
                    // should be removed
                    new Mission { Id = "expired", CreatedUtc = now.AddHours(-13), ExpiresIn = TimeSpan.FromHours(12),
                        CompletionTimeUtc = null },

                    // shouldn't be removed (locked)
                    new Mission { Id = "expiredButReadyToClaim", CreatedUtc = now.AddHours(-13), ExpiresIn = TimeSpan.FromHours(12),
                        CompletionTimeUtc = now.AddHours(1) },

                    // shouldn't be removed (locked)
                    new Mission { Id = "expiredButActive", CreatedUtc = now.AddHours(-13), ExpiresIn = TimeSpan.FromHours(12),
                        CompletionTimeUtc = now.AddHours(24) },

                    // shouldn't be removed (not expired)
                    new Mission { Id = "notExpired", CreatedUtc = now, ExpiresIn = TimeSpan.FromHours(12), CompletionTimeUtc = null }
                ]
            };

            var svc = new MissionExpirationService(state);

            int removed = svc.RemoveExpiredMissions();

            Assert.Equal(1, removed);
            Assert.DoesNotContain(state.Missions, m => m.Id == "expired");
            Assert.Contains(state.Missions, m => m.Id == "expiredButReadyToClaim");
            Assert.Contains(state.Missions, m => m.Id == "expiredButActive");
            Assert.Contains(state.Missions, m => m.Id == "notExpired");
        }
    }
}
