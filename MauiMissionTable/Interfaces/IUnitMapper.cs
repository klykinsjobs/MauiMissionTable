using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IUnitMapper
    {
        Unit FromSnapshot(UnitSnapshot snapshot);
        UnitSnapshot ToSnapshot(Unit unit);
    }
}
