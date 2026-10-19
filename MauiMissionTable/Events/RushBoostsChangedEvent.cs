using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record RushBoostsChangedEvent(int NewRushBoosts) : IEvent;
}
