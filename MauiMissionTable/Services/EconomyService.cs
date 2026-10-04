using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class EconomyService(GameState state, IGameStateSaver saver, IEventBus bus, IBoostService boosts,
        IPackOpeningService packs, IPassiveIncomeService passive) : IEconomyService
    {
        public async Task AddGoldAsync(int amount, CancellationToken cancellationToken = default)
        {
            if (amount < 0)
                return;

            state.Gold += amount;
            bus.Publish(new GoldChangedEvent(state.Gold));

            await saver.SaveAsync(cancellationToken);
        }

        public async Task<bool> SpendGoldAsync(int amount, CancellationToken cancellationToken = default)
        {
            if (amount <= 0)
                return false;

            if (state.Gold < amount)
                return false;

            state.Gold -= amount;
            bus.Publish(new GoldChangedEvent(state.Gold));

            await saver.SaveAsync(cancellationToken);
            return true;
        }

        public Task CollectPassiveGoldAsync(CancellationToken cancellationToken = default)
            => passive.CollectPassiveGoldAsync(cancellationToken);

        public Task<IReadOnlyList<PackReward>> OpenPackAsync(CancellationToken cancellationToken = default)
            => packs.OpenPackAsync(cancellationToken);

        public Task<bool> UseXpBoostAsync(Unit unit, CancellationToken cancellationToken = default)
            => boosts.UseXpBoostAsync(unit, cancellationToken);

        public Task<bool> RushMissionAsync(Mission mission, CancellationToken cancellationToken = default)
            => boosts.RushMissionAsync(mission, cancellationToken);

        public Task<bool> BuyPackWithGoldAsync(int cost, CancellationToken cancellationToken = default)
            => packs.BuyPackWithGoldAsync(cost, cancellationToken);

        public Task<bool> BuyRushBoostAsync(int costPremium, CancellationToken cancellationToken = default)
            => boosts.BuyRushBoostAsync(costPremium, cancellationToken);

        public Task<bool> BuyXpBoostAsync(int costPremium, CancellationToken cancellationToken = default)
            => boosts.BuyXpBoostAsync(costPremium, cancellationToken);

        public Task<bool> UpgradePassiveCapacityAsync(int costPremium, int increment, CancellationToken cancellationToken = default)
            => boosts.UpgradePassiveCapacityAsync(costPremium, increment, cancellationToken);
    }
}
