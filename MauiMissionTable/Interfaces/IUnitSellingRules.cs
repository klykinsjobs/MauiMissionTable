using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IUnitSellingRules
    {
        bool CanSellUnit(Unit unit);
        int CalculateSellValue(Unit unit);
    }
}
