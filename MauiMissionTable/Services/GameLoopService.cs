using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Services
{
    public class GameLoopService(IClock clock, IEventBus bus, TimeSpan? tickRate = null)
        : IGameLoopService, IAsyncDisposable
    {
        private readonly TimeSpan _tickRate = tickRate ?? TimeSpan.FromSeconds(1);
        private CancellationTokenSource? _cts;
        private Task? _loopTask;

        public void Start(CancellationToken cancellationToken = default)
        {
            // Ensure previous loop is fully cleaned up
            _cts?.Cancel();
            _cts?.Dispose();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _loopTask = Task.Run(() => LoopAsync(_cts.Token), _cts.Token);
        }

        private async Task LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_tickRate, token);
                    bus.Publish(new TickEvent(clock.UtcNow));
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_cts is not null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }

            if (_loopTask is not null)
            {
                try { await _loopTask; }
                catch { /* ignore */ }
            }

            GC.SuppressFinalize(this);
        }
    }
}
