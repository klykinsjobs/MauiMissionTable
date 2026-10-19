using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class UnitService(GameState state, IGameStateSaver saver, IEventBus bus, IToastService toastService,
        IUnitSellingRules sellingRules) : IUnitService
    {
        public IReadOnlyList<Unit> Units => [.. state.Units];

        public void AddUnit(Unit unit)
        {
            state.Units.Add(unit);
            bus.Publish(new UnitAddedEvent(unit));
        }

        public void NotifyUnitUpdated(Unit unit)
        {
            bus.Publish(new UnitUpdatedEvent(unit));
        }

        public async Task<bool> SellUnitAsync(Unit unit, CancellationToken cancellationToken = default)
        {
            if (!sellingRules.CanSellUnit(unit))
                return false;

            if (!state.Units.Remove(unit))
                return false;

            int sellValue = sellingRules.CalculateSellValue(unit);
            state.Gold += sellValue;

            bus.Publish(new GoldChangedEvent(state.Gold));
            bus.Publish(new UnitRemovedEvent(unit));

            await toastService.ShowAsync($"Sold {unit.Title} for {sellValue} gold");
            await saver.SaveAsync(cancellationToken);

            return true;
        }
    }
}
