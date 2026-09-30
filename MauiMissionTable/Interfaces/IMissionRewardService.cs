using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IMissionRewardService
    {
        bool RollSuccess(int successChance);

        Task ApplySuccessRewardsAsync(Mission mission, List<Unit> units, CancellationToken cancellationToken);
        Task ApplyFailureRewardsAsync(Mission mission, List<Unit> units, CancellationToken cancellationToken);
    }
}
