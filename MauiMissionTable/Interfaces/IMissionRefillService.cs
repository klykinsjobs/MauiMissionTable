namespace MauiMissionTable.Interfaces
{
    public interface IMissionRefillService
    {
        bool TryRefillMissions(IMissionGenerator generator, out int addedCount);
    }
}
