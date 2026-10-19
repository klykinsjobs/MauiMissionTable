using MauiMissionTable.Services;

namespace MauiMissionTableTests.Services
{
    public class RandomProviderTests
    {
        [Fact]
        public void Next_WithSeed_IsDeterministic()
        {
            var a = new RandomProvider(seed: 123);
            var b = new RandomProvider(seed: 123);

            for (int i = 0; i < 10; i++)
                Assert.Equal(a.Next(0, 100), b.Next(0, 100));
        }

        [Fact]
        public void NextDouble_WithSeed_IsDeterministic()
        {
            var c = new RandomProvider(seed: 42);
            var d = new RandomProvider(seed: 42);

            for (int i = 0; i < 10; i++)
                Assert.Equal(c.NextDouble(), d.NextDouble());
        }
    }
}
