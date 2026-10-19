using MauiMissionTable.Enums;
using MauiMissionTable.Models;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class UnitSellingRulesTests
    {
        [Fact]
        public void CanSellUnit_ReturnsFalse_WhenAssignedToMission()
        {
            var state = new GameState { Missions = [new Mission { AssignedUnitIds = ["u1"] }] };
            var rules = new UnitSellingRules(state);
            var unit = new Unit { Id = "u1" };

            Assert.False(rules.CanSellUnit(unit));
        }

        [Theory]
        [InlineData(Rarity.Common, 5)]
        [InlineData(Rarity.Uncommon, 7)]
        [InlineData(Rarity.Rare, 15)]
        [InlineData(Rarity.Epic, 30)]
        [InlineData(Rarity.Legendary, 60)]
        public void CalculateSellValue_ReturnsExpected_ForRarity(Rarity rarity, int expectedApprox)
        {
            var rules = new UnitSellingRules(new GameState());
            var unit = new Unit { Rarity = rarity };

            int value = rules.CalculateSellValue(unit);

            Assert.Equal(expectedApprox, value);
        }
    }
}
