using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IUnitLevelingService
    {
        void AddXp(Unit unit, int amount);
    }
}
