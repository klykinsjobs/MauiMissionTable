using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record XpBoostsChangedEvent(int NewXpBoosts) : IEvent;
}
