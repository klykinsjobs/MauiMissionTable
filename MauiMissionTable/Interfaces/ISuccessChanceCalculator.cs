using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface ISuccessChanceCalculator
    {
        int CalculateSuccessChance(Mission mission, IEnumerable<Unit> units);
        List<Unit> AutoAssignBestUnits(Mission mission, List<Unit> units);
    }
}
