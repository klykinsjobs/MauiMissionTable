using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class PassiveIncomeService : IPassiveIncomeService
    {
        private readonly GameState _state;
        private readonly IGameStateSaver _saver;
        private readonly IEventBus _bus;
        private readonly IToastService _toastService;

        public PassiveIncomeService(GameState state, IGameStateSaver saver, IEventBus bus, IToastService toastService)
        {
            _state = state;
            _saver = saver;
            _bus = bus;
            _toastService = toastService;

            _bus.Subscribe<TickEvent>(OnTick);
        }

        private void OnTick(TickEvent tick)
        {
            if (_state.PassiveGold < _state.PassiveGoldMax)
            {
                _state.PassiveGold++;
                _bus.Publish(new PassiveGoldChangedEvent(_state.PassiveGold));
            }
        }

        public async Task CollectPassiveGoldAsync(CancellationToken token)
        {
            if (_state.PassiveGold <= 0)
                return;

            _state.Gold += _state.PassiveGold;
            _state.PassiveGold = 0;

            _bus.Publish(new GoldChangedEvent(_state.Gold));
            _bus.Publish(new PassiveGoldChangedEvent(_state.PassiveGold));

            await _toastService.ShowAsync("Collected passive gold");
            await _saver.SaveAsync(token);

            await Task.CompletedTask;
        }
    }
}
