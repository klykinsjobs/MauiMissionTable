using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class GameStateSaver(GameState state, IGameRepository repository) : IGameStateSaver
    {
        private readonly SemaphoreSlim _saveLock = new(1, 1);

        public async Task SaveAsync(CancellationToken token = default)
        {
            await _saveLock.WaitAsync(token);
            try
            {
                await repository.SaveAsync(state, token);
            }
            finally
            {
                _saveLock.Release();
            }
        }
    }
}
