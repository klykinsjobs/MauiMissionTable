using MauiMissionTable.Models;
using MauiMissionTable.ViewModels;

namespace MauiMissionTableTests.Rows
{
    public class UnitRowTests
    {
        [Fact]
        public void Refresh_RaisesPropertyChanged()
        {
            var unit = new Unit { Title = "A", Level = 1 };
            var row = new UnitRow(unit);

            bool raised = false;
            row.PropertyChanged += (_, __) => raised = true;

            row.Refresh();

            Assert.True(raised);
        }
    }
}
