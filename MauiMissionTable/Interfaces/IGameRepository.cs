using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IGameRepository
    {
        Task<GameState> LoadAsync(CancellationToken cancellationToken = default);
        Task SaveAsync(GameState state, CancellationToken cancellationToken = default);
    }
}
