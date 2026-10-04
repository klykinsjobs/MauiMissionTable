using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class MissionRefillService(GameState state, IClock clock, TimeSpan? interval = null) : IMissionRefillService
    {
        private readonly TimeSpan _interval = interval ?? TimeSpan.FromSeconds(90);
        private DateTime _lastRefill = DateTime.MinValue;

        public bool TryRefillMissions(IMissionGenerator generator, out int addedCount)
        {
            addedCount = 0;

            // Too soon to refill
            if (clock.UtcNow - _lastRefill < _interval)
                return false;

            _lastRefill = clock.UtcNow;

            if (state.Missions.Count >= state.MaxMissions)
                return false;

            while (state.Missions.Count < state.MaxMissions)
            {
                state.Missions.Add(generator.Generate());
                addedCount++;
            }

            return true;
        }
    }
}
