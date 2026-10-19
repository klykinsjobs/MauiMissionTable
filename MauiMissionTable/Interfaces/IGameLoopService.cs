namespace MauiMissionTable.Interfaces
{
    public interface IGameLoopService : IAsyncDisposable
    {
        void Start(CancellationToken cancellationToken = default);
    }
}
