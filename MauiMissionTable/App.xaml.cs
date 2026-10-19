using MauiMissionTable.Interfaces;

namespace MauiMissionTable
{
    public partial class App : Application
    {
        public App(IGameStateService gameStateService)
        {
            InitializeComponent();

            _ = gameStateService.InitializeAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}