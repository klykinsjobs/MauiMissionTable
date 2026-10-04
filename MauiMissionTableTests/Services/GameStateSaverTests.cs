using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class GameStateSaverTests
    {
        [Fact]
        public async Task SaveAsync_CallsRepositorySaveAsync()
        {
            var state = new GameState { Gold = 5 };
            var mockRepo = new Mock<IGameRepository>();
            var saver = new GameStateSaver(state, mockRepo.Object);

            await saver.SaveAsync(CancellationToken.None);

            mockRepo.Verify(r => r.SaveAsync(state, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
