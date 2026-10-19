using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Mappers
{
    public class MissionMapper : IMissionMapper
    {
        public Mission FromSnapshot(MissionSnapshot s)
        {
            ArgumentNullException.ThrowIfNull(s);

            return new()
            {
                Id = s.Id,
                Title = s.Title,
                Rarity = s.Rarity,
                Level = s.Level,
                Duration = s.Duration,
                CreatedUtc = s.CreatedUtc,
                ExpiresIn = s.ExpiresIn,
                CompletionTimeUtc = s.CompletionTimeUtc,
                XpReward = s.XpReward,
                GoldReward = s.GoldReward,
                RushBoosts = s.RushBoosts,
                XpBoosts = s.XpBoosts,
                PremiumReward = s.PremiumReward,
                Packs = s.Packs,
                AssignedUnitIds = [.. s.AssignedUnitIds]
            };
        }

        public MissionSnapshot ToSnapshot(Mission m)
        {
            ArgumentNullException.ThrowIfNull(m);

            return new()
            {
                Id = m.Id,
                Title = m.Title,
                Rarity = m.Rarity,
                Level = m.Level,
                Duration = m.Duration,
                CreatedUtc = m.CreatedUtc,
                ExpiresIn = m.ExpiresIn,
                CompletionTimeUtc = m.CompletionTimeUtc,
                XpReward = m.XpReward,
                GoldReward = m.GoldReward,
                RushBoosts = m.RushBoosts,
                XpBoosts = m.XpBoosts,
                PremiumReward = m.PremiumReward,
                Packs = m.Packs,
                AssignedUnitIds = [.. m.AssignedUnitIds]
            };
        }
    }
}
