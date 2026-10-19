using CommunityToolkit.Mvvm.ComponentModel;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.ViewModels
{
    public partial class MissionRow(Mission mission, IClock clock) : ObservableObject
    {
        public Mission Mission { get; } = mission;

        public string Status => Mission.IsActive ? "In progress" : Mission.IsReadyToClaim ? "Done" : "Ready";

        public double Progress
        {
            get
            {
                if (Mission.CompletionTimeUtc is null)
                    return 0;

                var now = clock.UtcNow;
                var end = Mission.CompletionTimeUtc.Value;
                var start = end - Mission.Duration;

                if (now >= end)
                    return 1;

                var total = Mission.Duration.TotalSeconds;
                if (total <= 0)
                    return 1;

                return Math.Clamp((now - start).TotalSeconds / total, 0, 1);
            }
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(Progress));
        }
    }
}
