using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record PassiveGoldMaxChangedEvent(int NewPassiveGoldMax) : IEvent;
}
