using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IGameStateMapper
    {
        GameState FromSnapshot(GameStateSnapshot snapshot);
        GameStateSnapshot ToSnapshot(GameState state);
    }
}
