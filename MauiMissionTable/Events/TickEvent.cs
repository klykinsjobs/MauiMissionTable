using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record TickEvent(DateTime UtcNow) : IEvent;
}
