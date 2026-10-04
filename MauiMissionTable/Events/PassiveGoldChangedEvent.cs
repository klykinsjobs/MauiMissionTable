using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record PassiveGoldChangedEvent(int NewPassiveGold) : IEvent;
}
