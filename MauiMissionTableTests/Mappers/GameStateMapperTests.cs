using MauiMissionTable.Mappers;
using MauiMissionTable.Models;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Mappers
{
    public class GameStateMapperTests
    {
        [Fact]
        public void ToSnapshot_And_FromSnapshot_PreservesFields()
        {
            var mapper = new GameStateMapper(new UnitMapper(), new MissionMapper());
            var original = GameStateFactory.CreateDefault();
            original.Gold = 777;
            original.Units.Add(new Unit { Id = "u1", Title = "U1", Level = 2 });
            original.Missions.Add(new Mission { Id = "m1", Title = "M1", Level = 3 });

            var snapshot = mapper.ToSnapshot(original);
            var restored = mapper.FromSnapshot(snapshot);

            Assert.Equal(original.Gold, restored.Gold);
            Assert.Single(restored.Units);
            Assert.Single(restored.Missions);
            Assert.Equal("u1", restored.Units.First().Id);
            Assert.Equal("m1", restored.Missions.First().Id);
        }

        [Fact]
        public void FromSnapshot_Throws_WhenSnapshotIsNull()
        {
            var mapper = new GameStateMapper(new UnitMapper(), new MissionMapper());

            Assert.Throws<ArgumentNullException>(() => mapper.FromSnapshot(null!));
        }

        [Fact]
        public void ToSnapshot_Throws_WhenStateIsNull()
        {
            var mapper = new GameStateMapper(new UnitMapper(), new MissionMapper());

            Assert.Throws<ArgumentNullException>(() => mapper.ToSnapshot(null!));
        }
    }
}
