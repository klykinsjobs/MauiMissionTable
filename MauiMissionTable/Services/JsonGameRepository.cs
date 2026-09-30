using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using System.Text.Json;

namespace MauiMissionTable.Services
{
    public class JsonGameRepository : IGameRepository
    {
        private readonly IGameStateMapper _gameStateMapper;
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonGameRepository(IGameStateMapper gameStateMapper, string? filePath = null)
        {
            _gameStateMapper = gameStateMapper;
            _filePath = filePath ?? Path.Combine(FileSystem.AppDataDirectory, "game_state.json");
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<GameState> LoadAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (!File.Exists(_filePath))
                    return GameStateFactory.CreateDefault();

                await using var stream = File.OpenRead(_filePath);
                var snapshot = await JsonSerializer.DeserializeAsync<GameStateSnapshot>(stream, _options, cancellationToken);

                if (snapshot is null)
                    return GameStateFactory.CreateDefault();

                return _gameStateMapper.FromSnapshot(snapshot);
            }
            catch
            {
                return GameStateFactory.CreateDefault();
            }
        }

        public async Task SaveAsync(GameState state, CancellationToken cancellationToken = default)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

                var snapshot = _gameStateMapper.ToSnapshot(state);

                await using var stream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(stream, snapshot, _options, cancellationToken);
            }
            catch { /* ignore */ }
        }
    }
}
