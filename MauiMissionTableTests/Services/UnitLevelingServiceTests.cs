using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class UnitLevelingServiceTests
    {
        [Fact]
        public void AddXp_LevelsUp_WhenThresholdReached()
        {
            var unit = new Unit { Level = 1, Xp = 0 };
            var leveling = new UnitLevelingService(Mock.Of<IGameStateSaver>(), Mock.Of<IUnitService>());

            leveling.AddXp(unit, 150);  // 100 xp to level 2, 50 xp remainder

            Assert.Equal(2, unit.Level);
            Assert.Equal(50, unit.Xp);
        }

        [Fact]
        public void AddXp_LevelsUpMultipleTimes_WhenXpAmountLargeEnough()
        {
            var unit = new Unit { Level = 1, Xp = 0 };
            var leveling = new UnitLevelingService(Mock.Of<IGameStateSaver>(), Mock.Of<IUnitService>());

            leveling.AddXp(unit, 400);  // 100 xp to level 2, 200 xp to level 3, 100 xp remainder

            Assert.Equal(3, unit.Level);
            Assert.Equal(100, unit.Xp);
        }

        [Theory]
        [InlineData(10)]    // no level up
        [InlineData(100)]   // 1 level up
        [InlineData(1000)]  // multiple level ups
        public void AddXp_NotifyUnitUpdatedJustOnce_ForAnyAmountOfLevelUps(int xp)
        {
            var unit = new Unit { Level = 1, Xp = 0 };
            var mockUnits = new Mock<IUnitService>();
            var leveling = new UnitLevelingService(Mock.Of<IGameStateSaver>(), mockUnits.Object);

            leveling.AddXp(unit, xp);

            mockUnits.Verify(m => m.NotifyUnitUpdated(unit), Times.Once);
        }

        [Theory]
        [InlineData(10)]    // no level up
        [InlineData(100)]   // 1 level up
        [InlineData(1000)]  // multiple level ups
        public void AddXp_SaveAsyncJustOnce_ForAnyAmountOfLevelUps(int xp)
        {
            var unit = new Unit { Level = 1, Xp = 0 };
            var mockSaver = new Mock<IGameStateSaver>();
            var leveling = new UnitLevelingService(mockSaver.Object, Mock.Of<IUnitService>());

            leveling.AddXp(unit, xp);

            mockSaver.Verify(s => s.SaveAsync(), Times.Once);
        }
    }
}
