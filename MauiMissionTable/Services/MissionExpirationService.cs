using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class MissionExpirationService(GameState state) : IMissionExpirationService
    {
        public int RemoveExpiredMissions()
        {
            if (state.Missions.Count == 0)
                return 0;

            var expired = state.Missions
                .Where(m => m.IsExpired && (!m.IsLocked || m.IsReadyToClaim))
                .ToList();

            if (expired.Count == 0)
                return 0;

            foreach (var m in expired)
                state.Missions.Remove(m);

            return expired.Count;
        }
    }
}
