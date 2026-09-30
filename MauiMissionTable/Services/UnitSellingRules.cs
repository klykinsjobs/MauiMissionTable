using MauiMissionTable.Enums;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class UnitSellingRules(GameState state) : IUnitSellingRules
    {
        public bool CanSellUnit(Unit unit)
            => !state.Missions.Any(m => m.AssignedUnitIds.Contains(unit.Id));

        public int CalculateSellValue(Unit unit)
        {
            double rarityMultiplier = unit.Rarity switch
            {
                Rarity.Common => 1.0,
                Rarity.Uncommon => 1.5,
                Rarity.Rare => 3.0,
                Rarity.Epic => 6.0,
                Rarity.Legendary => 12.0,
                _ => 1.0
            };

            return (int)(5 * rarityMultiplier);
        }
    }
}
