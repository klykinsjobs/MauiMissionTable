namespace MauiMissionTable.Interfaces
{
    public interface IClock
    {
        DateTime UtcNow { get; }
        DateOnly Today { get; }
    }
}
