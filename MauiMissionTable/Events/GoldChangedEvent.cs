using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record GoldChangedEvent(int NewGold) : IEvent;
}
