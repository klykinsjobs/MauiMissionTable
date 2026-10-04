using CommunityToolkit.Maui;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Mappers;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using MauiMissionTable.ViewModels;
using MauiMissionTable.Views;
using Microsoft.Extensions.Logging;

namespace MauiMissionTable
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            if (OperatingSystem.IsAndroidVersionAtLeast(21, 0) || OperatingSystem.IsIOSVersionAtLeast(15, 0)
                || OperatingSystem.IsMacCatalystVersionAtLeast(15, 0) || OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763))
            {
                builder.UseMauiCommunityToolkit(options =>
                {
                    options.SetShouldEnableSnackbarOnWindows(true);
                });
            }

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Core Infrastructure
            builder.Services.AddSingleton<IClock, SystemClock>();
            builder.Services.AddSingleton<IRandomProvider, RandomProvider>();
            builder.Services.AddSingleton<IToastService, MauiToastService>();
            builder.Services.AddSingleton<IDialogService, MauiDialogService>();
            builder.Services.AddSingleton<IEventBus, EventBus>();

            // Persistence
            builder.Services.AddSingleton<GameStateFactory>();
            builder.Services.AddSingleton<IUnitMapper, UnitMapper>();
            builder.Services.AddSingleton<IMissionMapper, MissionMapper>();
            builder.Services.AddSingleton<IGameStateMapper, GameStateMapper>();
            builder.Services.AddSingleton<IGameRepository, JsonGameRepository>();
            builder.Services.AddSingleton<IGameStateSaver, GameStateSaver>();

            // Domain State
            builder.Services.AddSingleton<GameState>();

            // Domain Services
            builder.Services.AddSingleton<IUnitService, UnitService>();
            builder.Services.AddSingleton<IUnitLevelingService, UnitLevelingService>();
            builder.Services.AddSingleton<IUnitSellingRules, UnitSellingRules>();

            builder.Services.AddSingleton<IMissionService, MissionService>();
            builder.Services.AddSingleton<IMissionGenerator, MissionGenerator>();
            builder.Services.AddSingleton<IMissionRefillService, MissionRefillService>();
            builder.Services.AddSingleton<IMissionExpirationService, MissionExpirationService>();
            builder.Services.AddSingleton<IMissionRewardService, MissionRewardService>();
            builder.Services.AddSingleton<ISuccessChanceCalculator, SuccessChanceCalculator>();

            builder.Services.AddSingleton<IEconomyService, EconomyService>();
            builder.Services.AddSingleton<IPackOpeningService, PackOpeningService>();
            builder.Services.AddSingleton<IBoostService, BoostService>();
            builder.Services.AddSingleton<IPassiveIncomeService, PassiveIncomeService>();
            builder.Services.AddSingleton<IDailyRewardService, DailyRewardService>();

            // Game Loop + Orchestrator
            builder.Services.AddSingleton<IGameLoopService, GameLoopService>();
            builder.Services.AddSingleton<IGameOrchestrator, GameOrchestrator>();

            // UI Services
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IGameStateService, GameStateService>();

            builder.Services.AddTransient<UnitsViewModel>();
            builder.Services.AddTransient<UnitDetailViewModel>();
            builder.Services.AddTransient<PacksViewModel>();
            builder.Services.AddTransient<MissionsViewModel>();
            builder.Services.AddTransient<MissionDetailViewModel>();
            builder.Services.AddTransient<InventoryViewModel>();
            builder.Services.AddTransient<ShopViewModel>();

            builder.Services.AddTransient<UnitsPage>();
            builder.Services.AddTransient<UnitDetailPage>();
            builder.Services.AddTransient<PacksPage>();
            builder.Services.AddTransient<MissionsPage>();
            builder.Services.AddTransient<MissionDetailPage>();
            builder.Services.AddTransient<InventoryPage>();
            builder.Services.AddTransient<ShopPage>();

            return builder.Build();
        }
    }
}
