using MauiMissionTable.Events;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class BoostService(GameState state, IGameStateSaver saver, IEventBus bus, IClock clock,
        IToastService toastService, IUnitLevelingService unitLevelingService) : IBoostService
    {
        public async Task<bool> UseXpBoostAsync(Unit unit, CancellationToken token)
        {
            if (state.XpBoosts <= 0)
                return false;

            state.XpBoosts--;
            bus.Publish(new XpBoostsChangedEvent(state.XpBoosts));

            int xpAmount = 250;
            unitLevelingService.AddXp(unit, xpAmount);

            await toastService.ShowAsync($"Used XP boost on {unit.Title} (+{xpAmount} XP)");
            await saver.SaveAsync(token);
            return true;
        }

        public async Task<bool> RushMissionAsync(Mission mission, CancellationToken token)
        {
            if (!mission.IsActive)
                return false;

            if (state.RushBoosts <= 0)
                return false;

            state.RushBoosts--;
            bus.Publish(new RushBoostsChangedEvent(state.RushBoosts));

            var reduction = TimeSpan.FromSeconds(300);

            if (mission.CompletionTimeUtc.HasValue)
            {
                mission.CompletionTimeUtc = mission.CompletionTimeUtc.Value - reduction;

                if (mission.CompletionTimeUtc < clock.UtcNow)
                    mission.CompletionTimeUtc = clock.UtcNow;
            }

            await toastService.ShowAsync("Mission rushed! Time reduced.");
            await saver.SaveAsync(token);
            return true;
        }

        public async Task<bool> BuyRushBoostAsync(int costPremium, CancellationToken token)
        {
            if (costPremium < 0)
                return false;

            if (state.Premium < costPremium)
                return false;

            state.Premium -= costPremium;
            state.RushBoosts++;

            bus.Publish(new PremiumChangedEvent(state.Premium));
            bus.Publish(new RushBoostsChangedEvent(state.RushBoosts));

            await toastService.ShowAsync("Bought 1 rush boost!");
            await saver.SaveAsync(token);
            return true;
        }

        public async Task<bool> BuyXpBoostAsync(int costPremium, CancellationToken token)
        {
            if (costPremium < 0)
                return false;

            if (state.Premium < costPremium)
                return false;

            state.Premium -= costPremium;
            state.XpBoosts++;

            bus.Publish(new PremiumChangedEvent(state.Premium));
            bus.Publish(new XpBoostsChangedEvent(state.XpBoosts));

            await toastService.ShowAsync("Bought 1 XP boost!");
            await saver.SaveAsync(token);
            return true;
        }

        public async Task<bool> UpgradePassiveCapacityAsync(int costPremium, int increment, CancellationToken token)
        {
            if (costPremium < 0 || increment <= 0)
                return false;

            if (state.Premium < costPremium)
                return false;

            state.Premium -= costPremium;
            state.PassiveGoldMax += increment;

            bus.Publish(new PremiumChangedEvent(state.Premium));
            bus.Publish(new PassiveGoldMaxChangedEvent(state.PassiveGoldMax));

            await toastService.ShowAsync($"Passive capacity increased to {state.PassiveGoldMax}");
            await saver.SaveAsync(token);
            return true;
        }
    }
}
