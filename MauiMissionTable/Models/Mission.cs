using CommunityToolkit.Mvvm.ComponentModel;
using MauiMissionTable.Enums;
using System.Text.Json.Serialization;

namespace MauiMissionTable.Models
{
    public partial class Mission : ObservableObject
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
        public partial TimeSpan Duration { get; set; }

        [ObservableProperty]
        public partial DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        [ObservableProperty]
        public partial TimeSpan? ExpiresIn { get; set; } = TimeSpan.FromHours(12);

        [ObservableProperty]
        public partial DateTime? CompletionTimeUtc { get; set; }

        [ObservableProperty]
        public partial int XpReward { get; set; }

        [ObservableProperty]
        public partial int GoldReward { get; set; }

        [ObservableProperty]
        public partial int RushBoosts { get; set; }

        [ObservableProperty]
        public partial int XpBoosts { get; set; }

        [ObservableProperty]
        public partial int PremiumReward { get; set; }

        [ObservableProperty]
        public partial int Packs { get; set; }

        [ObservableProperty]
        public partial List<string> AssignedUnitIds { get; set; } = new();

        [JsonIgnore]
        public bool IsActive => CompletionTimeUtc.HasValue && CompletionTimeUtc > DateTime.UtcNow;

        [JsonIgnore]
        public bool IsReadyToClaim => CompletionTimeUtc.HasValue && CompletionTimeUtc <= DateTime.UtcNow;

        [JsonIgnore]
        public bool IsLocked => CompletionTimeUtc.HasValue;

        [JsonIgnore]
        public bool IsExpired => ExpiresIn.HasValue && DateTime.UtcNow > CreatedUtc + ExpiresIn.Value;

        [JsonIgnore]
        public string DurationText => $"{(int)Duration.TotalHours}h {Duration.Minutes}m {Duration.Seconds}s";

        [JsonIgnore]
        public string RewardsSummary
        {
            get
            {
                var parts = new List<string>();

                if (GoldReward > 0)
                    parts.Add($"Gold: {GoldReward}");

                if (XpReward > 0)
                    parts.Add($"XP: {XpReward}");

                if (RushBoosts > 0)
                    parts.Add($"Rush Boosts: {RushBoosts}");

                if (XpBoosts > 0)
                    parts.Add($"XP Boosts: {XpBoosts}");

                if (PremiumReward > 0)
                    parts.Add($"Premium: {PremiumReward}");

                if (Packs > 0)
                    parts.Add($"Packs: {Packs}");

                return parts.Count == 0 ? "None" : string.Join(" ", parts);
            }
        }
    }
}
