using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class UnitLevelingService(IGameStateSaver saver, IUnitService units) : IUnitLevelingService
    {
        public void AddXp(Unit unit, int amount)
        {
            if (amount <= 0)
                return;

            unit.Xp += amount;

            while (unit.XpToNextLevel > 0 && unit.Xp >= unit.XpToNextLevel)
            {
                unit.Xp -= unit.XpToNextLevel;
                unit.Level++;
            }

            units.NotifyUnitUpdated(unit);

            saver.SaveAsync();
        }
    }
}
