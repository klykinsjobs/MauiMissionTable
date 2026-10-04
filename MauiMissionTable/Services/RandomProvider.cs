using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Services
{
    public class RandomProvider(int? seed = null) : IRandomProvider
    {
        private readonly Random _rng = seed.HasValue ? new Random(seed.Value) : new Random();

        public int Next(int min, int max) => _rng.Next(min, max);
        public double NextDouble() => _rng.NextDouble();
    }
}
