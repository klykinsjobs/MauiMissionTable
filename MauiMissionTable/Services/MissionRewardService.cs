using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class MissionRewardService(GameState state, IEventBus bus, IRandomProvider rng,
        IToastService toastService, IUnitLevelingService unitLevelingService) : IMissionRewardService
    {
        public bool RollSuccess(int successChance)
        {
            int roll = rng.Next(0, 100);
            return roll < successChance;
        }

        public async Task ApplySuccessRewardsAsync(Mission mission, List<Unit> units, CancellationToken cancellationToken)
        {
            foreach (var unit in units)
            {
                unitLevelingService.AddXp(unit, mission.XpReward);
            }

            var rewardMessages = new List<string>();

            if (mission.GoldReward > 0)
            {
                state.Gold += mission.GoldReward;
                bus.Publish(new GoldChangedEvent(state.Gold));
                rewardMessages.Add($"+{mission.GoldReward} gold");
            }
            if (mission.XpReward > 0)
            {
                rewardMessages.Add($"+{mission.XpReward} XP");
            }
            if (mission.RushBoosts > 0)
            {
                state.RushBoosts += mission.RushBoosts;
                bus.Publish(new RushBoostsChangedEvent(state.RushBoosts));
                rewardMessages.Add($"+{mission.RushBoosts} rush boosts");
            }
            if (mission.XpBoosts > 0)
            {
                state.XpBoosts += mission.XpBoosts;
                bus.Publish(new XpBoostsChangedEvent(state.XpBoosts));
                rewardMessages.Add($"+{mission.XpBoosts} XP boosts");
            }
            if (mission.PremiumReward > 0)
            {
                state.Premium += mission.PremiumReward;
                bus.Publish(new PremiumChangedEvent(state.Premium));
                rewardMessages.Add($"+{mission.PremiumReward} premium");
            }

            string rewards = string.Join(", ", rewardMessages);
            await toastService.ShowAsync($"Success! {rewards}");
        }

        public async Task ApplyFailureRewardsAsync(Mission mission, List<Unit> units, CancellationToken cancellationToken)
        {
            int partialXp = (int)(mission.XpReward * 0.1);
            foreach (var unit in units)
            {
                unitLevelingService.AddXp(unit, partialXp);
            }

            await toastService.ShowAsync($"Mission failed. +{partialXp} XP");
        }
    }
}
