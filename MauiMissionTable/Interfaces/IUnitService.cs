using MauiMissionTable.Models;

namespace MauiMissionTable.Interfaces
{
    public interface IUnitService
    {
        IReadOnlyList<Unit> Units { get; }

        void AddUnit(Unit unit);
        void NotifyUnitUpdated(Unit unit);

        Task<bool> SellUnitAsync(Unit unit, CancellationToken cancellationToken = default);
    }
}
