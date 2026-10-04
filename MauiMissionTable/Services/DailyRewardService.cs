using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class DailyRewardService : IDailyRewardService
    {
        private readonly GameState _state;
        private readonly IGameStateSaver _saver;
        private readonly IEventBus _bus;
        private readonly IClock _clock;
        private readonly IToastService _toastService;

        public DailyRewardService(GameState state, IGameStateSaver saver, IEventBus bus, IClock clock, IToastService toastService)
        {
            _state = state;
            _saver = saver;
            _bus = bus;
            _clock = clock;
            _toastService = toastService;

            _bus.Subscribe<TickEvent>(OnTick);
        }

        private void OnTick(TickEvent tick)
        {
            var today = _clock.UtcNow.Date;

            // Already claimed today
            if (_state.LastDailyPackUtc?.Date == today)
                return;

            _state.Packs++;
            _state.LastDailyPackUtc = _clock.UtcNow;

            _bus.Publish(new PacksChangedEvent(_state.Packs));

            _toastService.ShowAsync("Daily reward: +1 pack");
            _saver.SaveAsync();
        }
    }
}
