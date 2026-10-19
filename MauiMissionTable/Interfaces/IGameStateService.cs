using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IGameStateService
    {
        int Gold { get; }
        int PassiveGold { get; }
        int PassiveGoldMax { get; }

        int Premium { get; }
        int RushBoosts { get; }
        int XpBoosts { get; }
        int Packs { get; }
        
        IReadOnlyList<Unit> Units { get; }

        Task InitializeAsync();

        Task AddGoldAsync(int amount);
        Task<bool> SpendGoldAsync(int amount);
        Task CollectPassiveGoldAsync();

        Task<IReadOnlyList<PackReward>> OpenPackAsync();

        Task<bool> SellUnitAsync(Unit unit);

        Task<bool> UseXpBoostAsync(Unit unit);
        Task<bool> RushMissionAsync(Mission mission);

        Task<bool> BuyPackWithGoldAsync(int cost);
        Task<bool> BuyRushBoostAsync(int costPremium);
        Task<bool> BuyXpBoostAsync(int costPremium);

        Task<bool> UpgradePassiveCapacityAsync(int costPremium, int increment);
    }
}
