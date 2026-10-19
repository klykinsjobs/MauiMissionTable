using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Events
{
    public record PremiumChangedEvent(int NewPremium) : IEvent;
}
