using MauiMissionTable.Models;

namespace MauiMissionTableTests.Models
{
    public class UnitTests
    {
        [Theory]
        [InlineData(1, 100)]
        [InlineData(2, 200)]
        [InlineData(5, 500)]
        public void XpToNextLevel_DependsOnLevel(int level, int expected)
        {
            var unit = new Unit { Level = level };

            Assert.Equal(expected, unit.XpToNextLevel);
        }

        [Fact]
        public void XpProgress_ZeroWhenNoXp()
        {
            var unit = new Unit { Level = 1, Xp = 0 };

            Assert.Equal(0, unit.XpProgress);
        }

        [Fact]
        public void XpProgress_ComputedCorrectly()
        {
            var unit = new Unit { Level = 2, Xp = 50 }; // 200 to next level

            var progress = unit.XpProgress;

            Assert.Equal(0.25, progress, 3);
        }
    }
}
