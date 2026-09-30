using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class MissionService : IMissionService
    {
        private readonly GameState _state;
        private readonly IGameStateSaver _saver;
        private readonly IEventBus _bus;
        private readonly IClock _clock;
        private readonly IToastService _toastService;
        private readonly IMissionGenerator _generator;
        private readonly IMissionExpirationService _expiration;
        private readonly IMissionRefillService _refill;
        private readonly IMissionRewardService _rewards;
        private readonly ISuccessChanceCalculator _success;

        public IReadOnlyList<Mission> Missions => [.. _state.Missions];

        public MissionService(GameState state, IGameStateSaver saver, IEventBus bus, IClock clock,
            IToastService toastService, IMissionGenerator generator, IMissionExpirationService expiration,
            IMissionRefillService refill, IMissionRewardService rewards, ISuccessChanceCalculator success)
        {
            _state = state;
            _saver = saver;
            _bus = bus;
            _clock = clock;
            _toastService = toastService;
            _generator = generator;
            _expiration = expiration;
            _refill = refill;
            _rewards = rewards;
            _success = success;

            _bus.Subscribe<TickEvent>(OnTick);
        }

        public bool IsUnitAssigned(Unit unit) =>
            _state.Missions.Any(m => m.IsLocked && m.AssignedUnitIds.Contains(unit.Id));

        public Mission? GetMissionForUnit(Unit unit) =>
            _state.Missions.FirstOrDefault(m => m.IsLocked && m.AssignedUnitIds.Contains(unit.Id));

        public int CalculateSuccessChance(Mission mission, IEnumerable<Unit> units) =>
            _success.CalculateSuccessChance(mission, units);

        public List<Unit> AutoAssignBestUnits(Mission mission, List<Unit> units) =>
            _success.AutoAssignBestUnits(mission, units);

        public async Task StartMissionAsync(Mission mission, IEnumerable<Unit> assignedUnits, CancellationToken cancellationToken = default)
        {
            if (mission.IsActive) return;

            mission.AssignedUnitIds = [.. assignedUnits.Select(c => c.Id)];
            mission.CompletionTimeUtc = _clock.UtcNow + mission.Duration;

            _bus.Publish(new MissionUpdatedEvent(mission));
            await _saver.SaveAsync(cancellationToken);
        }

        public async Task ClaimMissionAsync(Mission mission, CancellationToken cancellationToken = default)
        {
            if (!mission.IsReadyToClaim) return;

            var units = _state.Units
                .Where(c => mission.AssignedUnitIds.Contains(c.Id))
                .ToList();

            int successChance = _success.CalculateSuccessChance(mission, [.. units]);
            bool success = _rewards.RollSuccess(successChance);

            if (success)
            {
                await _rewards.ApplySuccessRewardsAsync(mission, units, cancellationToken);
            }
            else
            {
                await _rewards.ApplyFailureRewardsAsync(mission, units, cancellationToken);
            }

            _state.Missions.Remove(mission);

            _bus.Publish(new MissionUpdatedEvent(mission));
            _bus.Publish(new MissionsChangedEvent());

            await _saver.SaveAsync(cancellationToken);
        }

        private void OnTick(TickEvent tick)
        {
            // Expiration
            var expired = _expiration.RemoveExpiredMissions();
            if (expired > 0)
            {
                _bus.Publish(new MissionsChangedEvent());
                _toastService.ShowAsync($"{expired} missions expired");
                _saver.SaveAsync();
            }

            // Refill
            if (_refill.TryRefillMissions(_generator, out int added))
            {
                if (added > 0)
                {
                    _bus.Publish(new MissionsChangedEvent());
                    _toastService.ShowAsync("New missions available!");
                    _saver.SaveAsync();
                }
            }

            // Refresh active missions
            foreach (var mission in _state.Missions)
            {
                if (!mission.IsLocked)
                    continue;

                _bus.Publish(new MissionUpdatedEvent(mission));
            }
        }
    }
}
