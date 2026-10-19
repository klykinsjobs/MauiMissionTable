using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Events
{
    public record UnitUpdatedEvent(Unit Unit) : IEvent;
}
