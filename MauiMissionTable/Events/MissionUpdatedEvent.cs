using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Events
{
    public record MissionUpdatedEvent(Mission Mission) : IEvent;
}
