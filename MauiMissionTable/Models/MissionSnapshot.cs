using MauiMissionTable.Enums;

namespace MauiMissionTable.Models
{
    public class MissionSnapshot
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public Rarity Rarity { get; set; }
        public int Level { get; set; }

        public TimeSpan Duration { get; set; }
        public DateTime CreatedUtc { get; set; }
        public TimeSpan? ExpiresIn { get; set; }
        public DateTime? CompletionTimeUtc { get; set; }

        public int XpReward { get; set; }
        public int GoldReward { get; set; }
        public int RushBoosts { get; set; }
        public int XpBoosts { get; set; }
        public int PremiumReward { get; set; }
        public int Packs { get; set; }

        public List<string> AssignedUnitIds { get; set; } = [];
    }
}
