using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record PacksChangedEvent(int NewPacks) : IEvent;
}
