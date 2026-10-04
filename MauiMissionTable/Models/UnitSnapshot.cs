using MauiMissionTable.Enums;

namespace MauiMissionTable.Models
{
    public class UnitSnapshot
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Rarity Rarity { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }

        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Lightness { get; set; }
    }
}
