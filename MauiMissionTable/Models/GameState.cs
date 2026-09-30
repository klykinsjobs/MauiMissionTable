using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiMissionTable.Models
{
    public partial class GameState : ObservableObject
    {
        [ObservableProperty]
        public partial int Gold { get; set; }

        [ObservableProperty]
        public partial int PassiveGold { get; set; }

        [ObservableProperty]
        public partial int PassiveGoldMax { get; set; } = 100;

        [ObservableProperty]
        public partial int RushBoosts { get; set; }

        [ObservableProperty]
        public partial int XpBoosts { get; set; }

        [ObservableProperty]
        public partial int Premium { get; set; }

        [ObservableProperty]
        public partial int Packs { get; set; }

        [ObservableProperty]
        public partial int MaxMissions { get; set; } = 6;

        [ObservableProperty]
        public partial DateTime? LastDailyPackUtc { get; set; }

        [ObservableProperty]
        public partial List<Unit> Units { get; set; } = new();

        [ObservableProperty]
        public partial List<Mission> Missions { get; set; } = new();
    }
}
