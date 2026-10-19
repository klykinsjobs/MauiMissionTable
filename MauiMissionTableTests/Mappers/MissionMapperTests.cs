using MauiMissionTable.Enums;
using MauiMissionTable.Mappers;
using MauiMissionTable.Models;

namespace MauiMissionTableTests.Mappers
{
    public class MissionMapperTests
    {
        [Fact]
        public void ToSnapshot_And_FromSnapshot_PreservesFields()
        {
            var mapper = new MissionMapper();
            var mission = new Mission
            {
                Id = "m1",
                Title = "M1",
                Rarity = Rarity.Rare,
                Level = 4,
                Duration = TimeSpan.FromMinutes(5),
                CreatedUtc = DateTime.UtcNow,
                ExpiresIn = TimeSpan.FromHours(12),
                CompletionTimeUtc = DateTime.UtcNow.AddMinutes(5),
                XpReward = 50,
                GoldReward = 20,
                RushBoosts = 1,
                XpBoosts = 0,
                PremiumReward = 0,
                Packs = 0,
                AssignedUnitIds = ["u1"]
            };

            var snap = mapper.ToSnapshot(mission);
            var restored = mapper.FromSnapshot(snap);

            Assert.Equal(mission.Id, restored.Id);
            Assert.Equal(mission.Title, restored.Title);
            Assert.Equal(mission.Rarity, restored.Rarity);
            Assert.Equal(mission.AssignedUnitIds.Count, restored.AssignedUnitIds.Count);
        }

        [Fact]
        public void FromSnapshot_Throws_WhenSnapshotIsNull()
        {
            var mapper = new MissionMapper();

            Assert.Throws<ArgumentNullException>(() => mapper.FromSnapshot(null!));
        }

        [Fact]
        public void ToSnapshot_Throws_WhenMissionIsNull()
        {
            var mapper = new MissionMapper();

            Assert.Throws<ArgumentNullException>(() => mapper.ToSnapshot(null!));
        }
    }
}
