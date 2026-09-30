using MauiMissionTable.Mappers;
using MauiMissionTable.Models;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class JsonGameRepositoryTests
    {
        [Fact]
        public async Task SaveAndLoad_PersistsSnapshot_ToTempFile()
        {
            var state = GameStateFactory.CreateDefault();
            state.Gold = 123;
            state.Units.Add(new Unit { Id = "u1", Title = "U1" });
            var gameStateMapper = new GameStateMapper(new UnitMapper(), new MissionMapper());

            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var tempFile = Path.Combine(tempDir, "game_state.json");
            var repo = new JsonGameRepository(gameStateMapper, tempFile);

            await repo.SaveAsync(state, CancellationToken.None);

            Assert.True(File.Exists(tempFile));

            var loaded = await repo.LoadAsync(CancellationToken.None);

            Assert.Equal(123, loaded.Gold);
            Assert.Single(loaded.Units);

            File.Delete(tempFile);
            Directory.Delete(tempDir);
        }

        [Fact]
        public async Task LoadAsync_ReturnsDefault_WhenFileMissing()
        {
            var gameStateMapper = new GameStateMapper(new UnitMapper(), new MissionMapper());

            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var tempFile = Path.Combine(tempDir, "missing_file.json");
            var repo = new JsonGameRepository(gameStateMapper, tempFile);

            var loaded = await repo.LoadAsync(CancellationToken.None);

            Assert.NotNull(loaded);
            Assert.Equal(GameStateFactory.CreateDefault().Gold, loaded.Gold);

            Directory.Delete(tempDir);
        }
    }
}
