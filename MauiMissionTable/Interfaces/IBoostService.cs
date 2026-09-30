using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IBoostService
    {
        Task<bool> UseXpBoostAsync(Unit unit, CancellationToken token);
        Task<bool> RushMissionAsync(Mission mission, CancellationToken token);

        Task<bool> BuyRushBoostAsync(int costPremium, CancellationToken token);
        Task<bool> BuyXpBoostAsync(int costPremium, CancellationToken token);

        Task<bool> UpgradePassiveCapacityAsync(int costPremium, int increment, CancellationToken token);
    }
}
