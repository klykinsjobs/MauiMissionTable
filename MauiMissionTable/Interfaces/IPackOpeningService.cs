using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IPackOpeningService
    {
        Task<IReadOnlyList<PackReward>> OpenPackAsync(CancellationToken token);
        Task<bool> BuyPackWithGoldAsync(int cost, CancellationToken token);
    }
}
