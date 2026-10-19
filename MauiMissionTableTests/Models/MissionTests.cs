using MauiMissionTable.Models;

namespace MauiMissionTableTests.Models
{
    public class MissionTests
    {
        [Fact]
        public void IsActive_TrueWhenCompletionInFuture()
        {
            var mission = new Mission { CompletionTimeUtc = DateTime.UtcNow.AddMinutes(5) };

            Assert.True(mission.IsActive);
            Assert.False(mission.IsReadyToClaim);
        }

        [Fact]
        public void IsReadyToClaim_TrueWhenCompletionPast()
        {
            var mission = new Mission { CompletionTimeUtc = DateTime.UtcNow.AddMinutes(-1) };

            Assert.True(mission.IsReadyToClaim);
            Assert.False(mission.IsActive);
        }

        [Fact]
        public void IsLocked_TrueWhenCompletionHasValue()
        {
            var mission = new Mission { CompletionTimeUtc = DateTime.UtcNow };

            Assert.True(mission.IsLocked);
        }

        [Fact]
        public void IsLocked_FalseWhenNoCompletion()
        {
            var mission = new Mission { CompletionTimeUtc = null };

            Assert.False(mission.IsLocked);
        }

        [Fact]
        public void IsExpired_TrueWhenPastExpiration()
        {
            var mission = new Mission { CreatedUtc = DateTime.UtcNow.AddHours(-13), ExpiresIn = TimeSpan.FromHours(12) };

            Assert.True(mission.IsExpired);
        }

        [Fact]
        public void IsExpired_FalseWhenNotPastExpiration()
        {
            var mission = new Mission { CreatedUtc = DateTime.UtcNow.AddHours(-1), ExpiresIn = TimeSpan.FromHours(12) };

            Assert.False(mission.IsExpired);
        }

        [Fact]
        public void DurationText_FormatsCorrectly()
        {
            var mission = new Mission { Duration = TimeSpan.FromSeconds(5025) };    // 5025 = 3600 + 1380 + 45 = 1h 23m 45s

            Assert.Equal("1h 23m 45s", mission.DurationText);
        }

        [Fact]
        public void RewardsSummary_FormatsCorrectly()
        {
            var mission = new Mission { GoldReward = 10, XpReward = 20, RushBoosts = 1 };

            Assert.Contains("Gold: 10", mission.RewardsSummary);
            Assert.Contains("XP: 20", mission.RewardsSummary);
            Assert.Contains("Rush Boosts: 1", mission.RewardsSummary);
        }
    }
}
