using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IGameOrchestrator : IAsyncDisposable
    {
        GameState State { get; }

        Task InitializeAsync(CancellationToken cancellationToken = default);

        Task AddGoldAsync(int amount, CancellationToken cancellationToken = default);
        Task<bool> SpendGoldAsync(int amount, CancellationToken cancellationToken = default);
        Task CollectPassiveGoldAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PackReward>> OpenPackAsync(CancellationToken cancellationToken = default);

        Task<bool> SellUnitAsync(Unit unit, CancellationToken cancellationToken = default);
        Task<bool> UseXpBoostAsync(Unit unit, CancellationToken cancellationToken = default);

        Task<bool> RushMissionAsync(Mission mission, CancellationToken cancellationToken = default);

        Task<bool> BuyPackWithGoldAsync(int cost, CancellationToken cancellationToken = default);
        Task<bool> BuyRushBoostAsync(int costPremium, CancellationToken cancellationToken = default);
        Task<bool> BuyXpBoostAsync(int costPremium, CancellationToken cancellationToken = default);
        Task<bool> UpgradePassiveCapacityAsync(int costPremium, int increment, CancellationToken cancellationToken = default);

        bool IsUnitAssigned(Unit unit);
        Mission? GetMissionForUnit(Unit unit);
        int CalculateSuccessChance(Mission mission, IEnumerable<Unit> units);
        List<Unit> AutoAssignBestUnits(Mission mission, List<Unit> units);
        Task StartMissionAsync(Mission mission, IEnumerable<Unit> assignedUnits, CancellationToken cancellationToken = default);
        Task ClaimMissionAsync(Mission mission, CancellationToken cancellationToken = default);
    }
}
