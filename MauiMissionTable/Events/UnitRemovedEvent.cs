using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Events
{
    public record UnitRemovedEvent(Unit Unit) : IEvent;
}
