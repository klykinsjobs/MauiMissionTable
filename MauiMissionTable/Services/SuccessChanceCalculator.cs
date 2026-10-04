using MauiMissionTable.Enums;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class SuccessChanceCalculator : ISuccessChanceCalculator
    {
        private static double GetUnitContribution(Mission mission, Unit unit)
        {
            double rarityMultiplier = unit.Rarity switch
            {
                Rarity.Common => 1.0,
                Rarity.Uncommon => 1.1,
                Rarity.Rare => 1.25,
                Rarity.Epic => 1.5,
                Rarity.Legendary => 2.0,
                _ => 1.0
            };

            double levelFactor = mission.Level <= 0 ? 1 : unit.Level / (double)mission.Level;

            return 33.33 * levelFactor * rarityMultiplier;
        }

        private static double GetRequiredContribution(Mission mission)
        {
            double basePenalty = mission.Rarity switch
            {
                Rarity.Common => 0,
                Rarity.Uncommon => -10,
                Rarity.Rare => -20,
                Rarity.Epic => -35,
                Rarity.Legendary => -50,
                _ => 0
            };

            return 100 - basePenalty;
        }

        public int CalculateSuccessChance(Mission mission, IEnumerable<Unit> units)
        {
            double totalContribution = units.Sum(unit => SuccessChanceCalculator.GetUnitContribution(mission, unit));
            double required = GetRequiredContribution(mission);

            double chance = (totalContribution / required) * 100.0;

            return (int)Math.Clamp(chance, 0, 100);
        }

        public List<Unit> AutoAssignBestUnits(Mission mission, List<Unit> units)
        {
            // Precompute contributions
            var contributions = units
                .Select(u => (Unit: u, Value: SuccessChanceCalculator.GetUnitContribution(mission, u)))
                .OrderByDescending(x => x.Value)
                .ToList();

            double target = GetRequiredContribution(mission);

            var dp = new Dictionary<double, List<Unit>>
        {
            { 0, new List<Unit>() }
        };

            foreach (var (unit, value) in contributions)
            {
                foreach (var kvp in dp.ToList())
                {
                    double newSum = kvp.Key + value;

                    if (!dp.ContainsKey(newSum))
                        dp[newSum] = [.. kvp.Value, unit];
                }
            }

            // Find the smallest list whose sum >= target
            var best = dp
                .Where(kvp => kvp.Key >= target)
                .OrderBy(kvp => kvp.Value.Count)
                .FirstOrDefault();

            if (best.Value != null)
                return best.Value;

            // Otherwise return best 3 contributors
            return [.. contributions.Take(3).Select(x => x.Unit)];
        }
    }
}
