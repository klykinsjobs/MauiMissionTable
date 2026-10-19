using MauiMissionTable.Enums;
using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class PackOpeningService(GameState state, IGameStateSaver saver, IEventBus bus, IRandomProvider rng,
        IToastService toastService, IUnitService units) : IPackOpeningService
    {
        public async Task<IReadOnlyList<PackReward>> OpenPackAsync(CancellationToken token)
        {
            if (state.Packs <= 0)
                return [];

            state.Packs--;
            bus.Publish(new PacksChangedEvent(state.Packs));

            var rewards = new List<PackReward>();

            for (int i = 0; i < 5; i++)
                rewards.Add(GenerateReward());

            await saver.SaveAsync(token);
            return [.. rewards];
        }

        public async Task<bool> BuyPackWithGoldAsync(int cost, CancellationToken token)
        {
            if (state.Gold < cost)
                return false;

            state.Gold -= cost;
            state.Packs++;

            bus.Publish(new GoldChangedEvent(state.Gold));
            bus.Publish(new PacksChangedEvent(state.Packs));

            await toastService.ShowAsync("Bought 1 pack!");
            await saver.SaveAsync(token);
            return true;
        }

        private PackReward GenerateReward()
        {
            int roll = rng.Next(0, 100);

            if (roll < 70)
                return GenerateUnitReward();

            if (roll < 80)
                return GenerateGoldReward();

            if (roll < 90)
                return GeneratePremiumReward();

            if (roll < 95)
                return GenerateRushBoostReward();

            return GenerateXpBoostReward();
        }

        private PackReward GenerateUnitReward()
        {
            var rarity = RandomRarity();
            var id = rng.Next(1, 99999);

            var unit = new Unit
            {
                Title = $"Unit #{id}",
                Rarity = rarity,
                Hue = rng.NextDouble() * 360,
                Saturation = 0.6,
                Lightness = 0.55
            };

            units.AddUnit(unit);

            return new PackReward
            {
                RewardType = PackRewardType.Unit,
                Unit = unit,
                Amount = 1,
                Rarity = rarity
            };
        }

        private PackReward GenerateGoldReward()
        {
            int roll = rng.Next(0, 100);
            int amount;
            Rarity rarity;

            if (roll < 70)
            {
                amount = rng.Next(10, 26);
                rarity = Rarity.Rare;
            }
            else if (roll < 95)
            {
                amount = rng.Next(25, 51);
                rarity = Rarity.Epic;
            }
            else
            {
                amount = rng.Next(50, 101);
                rarity = Rarity.Legendary;
            }

            state.Gold += amount;
            bus.Publish(new GoldChangedEvent(state.Gold));

            return new PackReward
            {
                RewardType = PackRewardType.Gold,
                Amount = amount,
                Rarity = rarity
            };
        }

        private PackReward GeneratePremiumReward()
        {
            int roll = rng.Next(0, 100);
            int amount;
            Rarity rarity;

            if (roll < 70)
            {
                amount = 1;
                rarity = Rarity.Rare;
            }
            else if (roll < 95)
            {
                amount = 2;
                rarity = Rarity.Epic;
            }
            else
            {
                amount = 3;
                rarity = Rarity.Legendary;
            }

            state.Premium += amount;
            bus.Publish(new PremiumChangedEvent(state.Premium));

            return new PackReward
            {
                RewardType = PackRewardType.Premium,
                Amount = amount,
                Rarity = rarity
            };
        }

        private PackReward GenerateRushBoostReward()
        {
            state.RushBoosts++;
            bus.Publish(new RushBoostsChangedEvent(state.RushBoosts));

            return new PackReward
            {
                RewardType = PackRewardType.RushBoost,
                Amount = 1,
                Rarity = Rarity.Uncommon
            };
        }

        private PackReward GenerateXpBoostReward()
        {
            state.XpBoosts++;
            bus.Publish(new XpBoostsChangedEvent(state.XpBoosts));

            return new PackReward
            {
                RewardType = PackRewardType.XpBoost,
                Amount = 1,
                Rarity = Rarity.Rare
            };
        }

        private Rarity RandomRarity()
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
