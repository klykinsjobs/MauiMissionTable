using MauiMissionTable.Enums;
using MauiMissionTable.Models;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class SuccessChanceCalculatorTests
    {
        [Fact]
        public void CalculateSuccessChance_NoUnits_MinimumClampToZero()
        {
            var calc = new SuccessChanceCalculator();
            var mission = new Mission { Level = 1, Rarity = Rarity.Legendary };

            int chance = calc.CalculateSuccessChance(mission, []);

            Assert.Equal(0, chance);
        }

        [Fact]
        public void AutoAssignBestUnits_ReturnsSingleUnit_WhenOneIsSufficient()
        {
            var calc = new SuccessChanceCalculator();
            var mission = new Mission { Level = 1, Rarity = Rarity.Common };
            var strong = new Unit { Level = 100, Rarity = Rarity.Legendary };
            var weak = new Unit { Level = 1, Rarity = Rarity.Common };

            var result = calc.AutoAssignBestUnits(mission, [weak, strong]);

            Assert.Single(result);
            Assert.Contains(strong, result);
        }

        [Fact]
        public void AutoAssignBestUnits_ReturnsUpToThreeUnits()
        {
            var calc = new SuccessChanceCalculator();
            var mission = new Mission { Level = 10, Rarity = Rarity.Legendary };
            var units = new List<Unit>
            {
                new() { Level = 1, Rarity = Rarity.Common },
                new() { Level = 2, Rarity = Rarity.Common },
                new() { Level = 3, Rarity = Rarity.Common },
                new() { Level = 4, Rarity = Rarity.Common }
            };

            var result = calc.AutoAssignBestUnits(mission, units);

            Assert.InRange(result.Count, 1, 3);
        }

        [Fact]
        public void CalculateSuccessChance_HandlesZeroMissionLevel_Gracefully()
        {
            var calc = new SuccessChanceCalculator();
            var mission = new Mission { Level = 0, Rarity = Rarity.Common };
            var unit = new Unit { Level = 1, Rarity = Rarity.Common };

            int chance = calc.CalculateSuccessChance(mission, [unit]);

            Assert.InRange(chance, 0, 100);
        }
    }
}
