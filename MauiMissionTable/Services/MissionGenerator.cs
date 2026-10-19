using MauiMissionTable.Enums;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class MissionGenerator(GameState state, IClock clock, IRandomProvider rng) : IMissionGenerator
    {
        public Mission Generate()
        {
            var id = rng.Next(1, 99999);

            int highestLevel = state.Units.Count == 0
                ? 1
                : state.Units.Max(c => c.Level);

            int level = rng.Next(1, highestLevel + 1);
            var rarity = GetRandomRarity();

            double baseSeconds = 30 + level * 10;
            double rarityFactor = rarity switch
            {
                Rarity.Common => 1.0,
                Rarity.Uncommon => 1.2,
                Rarity.Rare => 1.5,
                Rarity.Epic => 2.0,
                Rarity.Legendary => 2.5,
                _ => 1.0
            };
            var duration = TimeSpan.FromSeconds(baseSeconds * rarityFactor);

            int baseXp = level * 40;
            int baseGold = level * 12;

            double rewardMultiplier = rarity switch
            {
                Rarity.Common => 1.0,
                Rarity.Uncommon => 1.6,
                Rarity.Rare => 2.8,
                Rarity.Epic => 4.5,
                Rarity.Legendary => 7.5,
                _ => 1.0
            };

            int xpReward = (int)(baseXp * rewardMultiplier);
            int goldReward = (int)(baseGold * rewardMultiplier);

            return new Mission
            {
                Title = $"Mission #{id}",
                Level = level,
                Rarity = rarity,
                Duration = duration,
                XpReward = xpReward,
                GoldReward = goldReward,
                RushBoosts = rarity >= Rarity.Uncommon ? 1 : 0,
                XpBoosts = rarity >= Rarity.Rare ? 1 : 0,
                PremiumReward = rarity >= Rarity.Epic ? 1 : 0,
                Packs = rarity == Rarity.Legendary ? 1 : 0,
                CreatedUtc = clock.UtcNow
            };
        }

        private Rarity GetRandomRarity()
        {
            int roll = rng.Next(0, 100);

            if (roll < 60)
                return Rarity.Common;

            if (roll < 85)
                return Rarity.Uncommon;

            if (roll < 95)
                return Rarity.Rare;

            if (roll < 99)
                return Rarity.Epic;

            return Rarity.Legendary;
        }
    }
}
