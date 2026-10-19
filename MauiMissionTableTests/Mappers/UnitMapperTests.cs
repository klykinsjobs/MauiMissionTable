using MauiMissionTable.Enums;
using MauiMissionTable.Mappers;
using MauiMissionTable.Models;

namespace MauiMissionTableTests.Mappers
{
    public class UnitMapperTests
    {
        [Fact]
        public void ToSnapshot_And_FromSnapshot_PreservesFields()
        {
            var mapper = new UnitMapper();
            var unit = new Unit
            {
                Id = "u1",
                Title = "U1",
                Rarity = Rarity.Epic,
                Level = 5,
                Xp = 123,
                Hue = 180.0,
                Saturation = 0.5,
                Lightness = 0.4
            };

            var snap = mapper.ToSnapshot(unit);
            var restored = mapper.FromSnapshot(snap);

            Assert.Equal(unit.Id, restored.Id);
            Assert.Equal(unit.Title, restored.Title);
            Assert.Equal(unit.Rarity, restored.Rarity);
            Assert.Equal(unit.Level, restored.Level);
            Assert.Equal(unit.Xp, restored.Xp);
            Assert.Equal(unit.Hue, restored.Hue);
        }

        [Fact]
        public void FromSnapshot_Throws_WhenSnapshotIsNull()
        {
            var mapper = new UnitMapper();

            Assert.Throws<ArgumentNullException>(() => mapper.FromSnapshot(null!));
        }

        [Fact]
        public void ToSnapshot_Throws_WhenUnitIsNull()
        {
            var mapper = new UnitMapper();

            Assert.Throws<ArgumentNullException>(() => mapper.ToSnapshot(null!));
        }
    }
}
