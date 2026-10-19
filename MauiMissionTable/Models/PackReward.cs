using CommunityToolkit.Mvvm.ComponentModel;
using MauiMissionTable.Enums;
using System.Text.Json.Serialization;

namespace MauiMissionTable.Models
{
    public partial class PackReward : ObservableObject
    {
        [ObservableProperty]
        public partial PackRewardType RewardType { get; set; }

        [ObservableProperty]
        public partial Unit? Unit { get; set; }

        [ObservableProperty]
        public partial int Amount { get; set; }

        [ObservableProperty]
        public partial Rarity Rarity { get; set; }
        
        [JsonIgnore]
        public string Title => RewardType switch
        {
            PackRewardType.Unit => Unit?.Title ?? "Unit",
            PackRewardType.Gold => $"{Amount} Gold",
            PackRewardType.Premium => $"{Amount} Premium",
            PackRewardType.RushBoost => "Rush Boost",
            PackRewardType.XpBoost => "XP Boost",
            _ => "Reward"
        };

        [JsonIgnore]
        public Color Color => RewardType == PackRewardType.Unit && Unit != null
            ? Unit.Color
            : Colors.Transparent;
    }
}
