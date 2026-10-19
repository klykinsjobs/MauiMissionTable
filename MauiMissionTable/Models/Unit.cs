using CommunityToolkit.Mvvm.ComponentModel;
using MauiMissionTable.Enums;
using System.Text.Json.Serialization;

namespace MauiMissionTable.Models
{
    public partial class Unit : ObservableObject
    {
        [ObservableProperty]
        public partial string Id { get; set; } = Guid.NewGuid().ToString();

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Rarity Rarity { get; set; }

        [ObservableProperty]
        public partial int Level { get; set; } = 1;

        [ObservableProperty]
        public partial int Xp { get; set; } = 0;

        [ObservableProperty]
        public partial double Hue { get; set; }

        [ObservableProperty]
        public partial double Saturation { get; set; }

        [ObservableProperty]
        public partial double Lightness { get; set; }

        [JsonIgnore]
        public Color Color => Color.FromHsla(Hue / 360.0, Saturation, Lightness);

        [JsonIgnore]
        public int XpToNextLevel => Level * 100;

        [JsonIgnore]
        public double XpProgress => XpToNextLevel == 0 ? 0 : Xp / (double)XpToNextLevel;
    }
}
