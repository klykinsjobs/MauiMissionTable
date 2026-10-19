using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IMissionMapper
    {
        Mission FromSnapshot(MissionSnapshot snapshot);
        MissionSnapshot ToSnapshot(Mission mission);
    }
}
