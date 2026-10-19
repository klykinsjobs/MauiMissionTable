using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Events
{
    public record UnitAddedEvent(Unit Unit) : IEvent;
}
