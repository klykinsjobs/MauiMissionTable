using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class GameStateService(IGameOrchestrator orchestrator) : IGameStateService
    {
        public int Gold => orchestrator.State.Gold;
        public int PassiveGold => orchestrator.State.PassiveGold;
        public int PassiveGoldMax => orchestrator.State.PassiveGoldMax;
        public int Premium => orchestrator.State.Premium;
        public int RushBoosts => orchestrator.State.RushBoosts;
        public int XpBoosts => orchestrator.State.XpBoosts;
        public int Packs => orchestrator.State.Packs;

        public IReadOnlyList<Unit> Units => [.. orchestrator.State.Units];

        public Task InitializeAsync() => orchestrator.InitializeAsync();
        
        public Task AddGoldAsync(int amount) => orchestrator.AddGoldAsync(amount);
        public Task<bool> SpendGoldAsync(int amount) => orchestrator.SpendGoldAsync(amount);
        public Task CollectPassiveGoldAsync() => orchestrator.CollectPassiveGoldAsync();
        
        public Task<IReadOnlyList<PackReward>> OpenPackAsync() => orchestrator.OpenPackAsync();
        
        public Task<bool> SellUnitAsync(Unit unit) => orchestrator.SellUnitAsync(unit);

        public Task<bool> UseXpBoostAsync(Unit unit) => orchestrator.UseXpBoostAsync(unit);
        public Task<bool> RushMissionAsync(Mission mission) => orchestrator.RushMissionAsync(mission);

        public Task<bool> BuyPackWithGoldAsync(int cost) => orchestrator.BuyPackWithGoldAsync(cost);
        public Task<bool> BuyRushBoostAsync(int costPremium) => orchestrator.BuyRushBoostAsync(costPremium);
        public Task<bool> BuyXpBoostAsync(int costPremium) => orchestrator.BuyXpBoostAsync(costPremium);

        public Task<bool> UpgradePassiveCapacityAsync(int costPremium, int increment)
            => orchestrator.UpgradePassiveCapacityAsync(costPremium, increment);
    }
}
