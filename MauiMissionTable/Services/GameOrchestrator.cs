using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class GameOrchestrator(GameState state, IGameRepository repository, IGameLoopService loop,
        IMissionService missions, IUnitService units, IEconomyService economy,
        IDailyRewardService dailyRewards) : IGameOrchestrator
    {
        private readonly IDailyRewardService _dailyRewards = dailyRewards;

        public GameState State => state;

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var loaded = await repository.LoadAsync(cancellationToken);

            // Copy loaded values into the injected singleton state
            state.Gold = loaded.Gold;
            state.PassiveGold = loaded.PassiveGold;
            state.PassiveGoldMax = loaded.PassiveGoldMax;
            state.Packs = loaded.Packs;
            state.Units = loaded.Units;
            state.Missions = loaded.Missions;
            state.RushBoosts = loaded.RushBoosts;
            state.XpBoosts = loaded.XpBoosts;
            state.Premium = loaded.Premium;
            state.LastDailyPackUtc = loaded.LastDailyPackUtc;

            loop.Start(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await loop.DisposeAsync();
            
            GC.SuppressFinalize(this);
        }

        public Task AddGoldAsync(int amount, CancellationToken cancellationToken = default)
            => economy.AddGoldAsync(amount, cancellationToken);

        public Task<bool> SpendGoldAsync(int amount, CancellationToken cancellationToken = default)
            => economy.SpendGoldAsync(amount, cancellationToken);

        public Task CollectPassiveGoldAsync(CancellationToken cancellationToken = default)
            => economy.CollectPassiveGoldAsync(cancellationToken);

        public Task<IReadOnlyList<PackReward>> OpenPackAsync(CancellationToken cancellationToken = default)
            => economy.OpenPackAsync(cancellationToken);

        public Task<bool> SellUnitAsync(Unit unit, CancellationToken cancellationToken = default)
            => units.SellUnitAsync(unit, cancellationToken);

        public Task<bool> UseXpBoostAsync(Unit unit, CancellationToken cancellationToken = default)
            => economy.UseXpBoostAsync(unit, cancellationToken);

        public Task<bool> RushMissionAsync(Mission mission, CancellationToken cancellationToken = default)
            => economy.RushMissionAsync(mission, cancellationToken);

        public Task<bool> BuyPackWithGoldAsync(int cost, CancellationToken cancellationToken = default)
            => economy.BuyPackWithGoldAsync(cost, cancellationToken);

        public Task<bool> BuyRushBoostAsync(int costPremium, CancellationToken cancellationToken = default)
            => economy.BuyRushBoostAsync(costPremium, cancellationToken);

        public Task<bool> BuyXpBoostAsync(int costPremium, CancellationToken cancellationToken = default)
            => economy.BuyXpBoostAsync(costPremium, cancellationToken);

        public Task<bool> UpgradePassiveCapacityAsync(int costPremium, int increment, CancellationToken cancellationToken = default)
            => economy.UpgradePassiveCapacityAsync(costPremium, increment, cancellationToken);

        public bool IsUnitAssigned(Unit unit)
            => missions.IsUnitAssigned(unit);

        public Mission? GetMissionForUnit(Unit unit)
            => missions.GetMissionForUnit(unit);

        public int CalculateSuccessChance(Mission mission, IEnumerable<Unit> units)
            => missions.CalculateSuccessChance(mission, units);

        public List<Unit> AutoAssignBestUnits(Mission mission, List<Unit> units)
            => missions.AutoAssignBestUnits(mission, units);

        public Task StartMissionAsync(Mission mission, IEnumerable<Unit> assignedUnits, CancellationToken cancellationToken = default)
            => missions.StartMissionAsync(mission, assignedUnits, cancellationToken);

        public Task ClaimMissionAsync(Mission mission, CancellationToken cancellationToken = default)
            => missions.ClaimMissionAsync(mission, cancellationToken);
    }
}
