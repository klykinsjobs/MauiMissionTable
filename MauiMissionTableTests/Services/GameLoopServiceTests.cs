using MauiMissionTable.Events;
using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class GameLoopServiceTests
    {
        [Fact]
        public async Task Start_MultipleTimes_NoException()
        {
            var loop = new GameLoopService(new SystemClock(), new EventBus(), TimeSpan.FromMilliseconds(10));

            // Starting again should cancel previous and start new loop
            loop.Start();
            loop.Start();
            loop.Start();

            // Dispose should stop gracefully
            var disposeTask = loop.DisposeAsync().AsTask();
            await disposeTask;

            Assert.True(disposeTask.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task Start_PublishesTickEvents_Periodically()
        {
            var bus = new EventBus();
            var loop = new GameLoopService(new SystemClock(), bus, TimeSpan.FromMilliseconds(50));
            var timeout = TimeSpan.FromSeconds(1);
            int ticks = 0;
            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            bus.Subscribe<TickEvent>(_ =>
            {
                if (Interlocked.Increment(ref ticks) >= 3)
                    tcs.TrySetResult(null);
            });

            using var cts = new CancellationTokenSource();
            loop.Start(cts.Token);

            await tcs.Task.WaitAsync(timeout);

            cts.Cancel();
            await loop.DisposeAsync();

            Assert.True(ticks >= 3);
        }
    }
}
