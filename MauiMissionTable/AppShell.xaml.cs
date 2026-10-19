using MauiMissionTable.Views;

namespace MauiMissionTable
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(UnitDetailPage), typeof(UnitDetailPage));
            Routing.RegisterRoute(nameof(MissionDetailPage), typeof(MissionDetailPage));
        }
    }
}
