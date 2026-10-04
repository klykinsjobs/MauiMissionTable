namespace MauiMissionTable.Interfaces
{
    public interface IGameStateSaver
    {
        Task SaveAsync(CancellationToken token = default);
    }
}
