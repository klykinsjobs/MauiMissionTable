using MauiMissionTable.Models;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;

namespace MauiMissionTableTests.Rows
{
    public class MissionRowTests
    {
        [Fact]
        public void Refresh_RaisesPropertyChanged()
        {
            var clock = new SystemClock();
            var mission = new Mission { Duration = TimeSpan.FromSeconds(10) };
            var row = new MissionRow(mission, clock);

            bool raised = false;
            row.PropertyChanged += (_, __) => raised = true;

            row.Refresh();

            Assert.True(raised);
        }

        [Fact]
        public void Status_Ready_WhenNotStarted()
        {
            var clock = new SystemClock();
            var mission = new Mission { CompletionTimeUtc = null };
            var row = new MissionRow(mission, clock);

            Assert.Equal("Ready", row.Status);
        }

        [Fact]
        public void Status_InProgress_WhenStartedAndNotFinished()
        {
            var clock = new SystemClock();
            var mission = new Mission { CompletionTimeUtc = DateTime.UtcNow.AddMinutes(5), Duration = TimeSpan.FromMinutes(10) };
            var row = new MissionRow(mission, clock);

            Assert.Equal("In progress", row.Status);
        }

        [Fact]
        public void Status_Done_WhenStartedAndFinished()
        {
            var clock = new SystemClock();
            var mission = new Mission { CompletionTimeUtc = DateTime.UtcNow.AddMinutes(-1), Duration = TimeSpan.FromMinutes(10) };
            var row = new MissionRow(mission, clock);

            Assert.Equal("Done", row.Status);
        }

        [Fact]
        public void Progress_Zero_WhenNotStarted()
        {
            var clock = new SystemClock();
            var mission = new Mission { CompletionTimeUtc = null };
            var row = new MissionRow(mission, clock);

            Assert.Equal(0, row.Progress);
        }

        [Fact]
        public void Progress_CalculatesCorrectly_WhenStartedAndNotFinished()
        {
            var clock = new SystemClock();
            var mission = new Mission { CompletionTimeUtc = DateTime.UtcNow.AddMinutes(5), Duration = TimeSpan.FromMinutes(10) };
            var row = new MissionRow(mission, clock);

            double p = row.Progress;

            Assert.True(p > 0.49 && p < 0.51);
        }

        [Fact]
        public void Progress_One_WhenStartedAndFinished()
        {
            var clock = new SystemClock();
            var mission = new Mission { CompletionTimeUtc = DateTime.UtcNow.AddMinutes(-1), Duration = TimeSpan.FromMinutes(10) };
            var row = new MissionRow(mission, clock);

            Assert.Equal(1, row.Progress);
        }
    }
}
