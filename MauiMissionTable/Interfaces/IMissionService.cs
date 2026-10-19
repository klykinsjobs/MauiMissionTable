using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IMissionService
    {
        IReadOnlyList<Mission> Missions { get; }

        bool IsUnitAssigned(Unit unit);
        Mission? GetMissionForUnit(Unit unit);

        int CalculateSuccessChance(Mission mission, IEnumerable<Unit> units);
        List<Unit> AutoAssignBestUnits(Mission mission, List<Unit> units);

        Task StartMissionAsync(Mission mission, IEnumerable<Unit> assignedUnits, CancellationToken cancellationToken = default);
        Task ClaimMissionAsync(Mission mission, CancellationToken cancellationToken = default);
    }
}
