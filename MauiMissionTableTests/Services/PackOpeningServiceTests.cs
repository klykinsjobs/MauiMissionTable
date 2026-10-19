using MauiMissionTable.Enums;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class PackOpeningServiceTests
    {
        [Fact]
        public async Task OpenPackAsync_DecrementsPacksAndReturnsFiveRewards()
        {
            var state = new GameState { Packs = 1, Units = [] };
            var mockSaver = new Mock<IGameStateSaver>();
            var service = new PackOpeningService(state, mockSaver.Object, new EventBus(), new RandomProvider(),
                Mock.Of<IToastService>(), Mock.Of<IUnitService>());

            var rewards = await service.OpenPackAsync(CancellationToken.None);

            Assert.Equal(0, state.Packs);
            Assert.Equal(5, rewards.Count);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OpenPackAsync_ReturnsNoRewards_WhenNoPacks()
        {
            var state = new GameState { Packs = 0, Units = [] };
            var service = new PackOpeningService(state, Mock.Of<IGameStateSaver>(), new EventBus(), new RandomProvider(),
                Mock.Of<IToastService>(), Mock.Of<IUnitService>());

            var rewards = await service.OpenPackAsync(CancellationToken.None);

            Assert.Empty(rewards);
            Assert.Equal(0, state.Packs);
        }

        [Fact]
        public async Task BuyPackWithGoldAsync_Fails_WhenNotEnoughGold()
        {
            var state = new GameState { Gold = 10, Packs = 0 };
            var service = new PackOpeningService(state, Mock.Of<IGameStateSaver>(), new EventBus(), new RandomProvider(),
                Mock.Of<IToastService>(), Mock.Of<IUnitService>());

            bool result = await service.BuyPackWithGoldAsync(50, CancellationToken.None);

            Assert.False(result);
            Assert.Equal(0, state.Packs);
            Assert.Equal(10, state.Gold);
        }

        [Fact]
        public async Task BuyPackWithGoldAsync_IncrementsPacks_ShowsToastAndSaves()
        {
            var state = new GameState { Gold = 100, Packs = 0 };
            var mockToast = new Mock<IToastService>();
            var mockSaver = new Mock<IGameStateSaver>();
            var service = new PackOpeningService(state, mockSaver.Object, new EventBus(), new RandomProvider(), mockToast.Object,
                Mock.Of<IUnitService>());

            bool result = await service.BuyPackWithGoldAsync(50, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(50, state.Gold);
            Assert.Equal(1, state.Packs);
            mockToast.Verify(t => t.ShowAsync(It.Is<string>(s => s.Contains("Bought"))), Times.Once);
            mockSaver.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OpenPackAsync_AllUnitRewards_Path()
        {
            // Force unit path (roll < 70)
            var ints = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            var rng = new DeterministicRandom(ints);
            var state = new GameState { Packs = 1, Units = [] };
            var mockUnits = new Mock<IUnitService>();
            var service = new PackOpeningService(state, Mock.Of<IGameStateSaver>(), new EventBus(), rng, Mock.Of<IToastService>(),
                mockUnits.Object);

            var rewards = await service.OpenPackAsync(CancellationToken.None);

            Assert.Equal(0, state.Packs);
            Assert.Equal(5, rewards.Count);
            Assert.True(rewards.All(r => r.RewardType == PackRewardType.Unit));
            mockUnits.Verify(u => u.AddUnit(It.IsAny<Unit>()), Times.Exactly(5));
        }

        [Fact]
        public async Task OpenPackAsync_AllGoldRewards_Path()
        {
            // Force gold path (roll >= 70 and < 80)
            var ints = new[] { 75, 75, 75, 75, 75, 75, 75, 75, 75, 75, 75, 75, 75, 75, 75 };
            var rng = new DeterministicRandom(ints);
            var state = new GameState { Packs = 1, Gold = 0, Units = [] };
            var service = new PackOpeningService(state, Mock.Of<IGameStateSaver>(), new EventBus(), rng, Mock.Of<IToastService>(),
                Mock.Of<IUnitService>());

            var rewards = await service.OpenPackAsync(CancellationToken.None);

            Assert.Equal(0, state.Packs);
            Assert.Equal(5, rewards.Count);
            Assert.True(state.Gold > 0);
            Assert.True(rewards.All(r => r.RewardType == PackRewardType.Gold));
        }

        [Fact]
        public async Task OpenPackAsync_AllPremiumRewards_Path()
        {
            // Force premium path (roll >= 80 and < 90)
            var ints = new[] { 85, 85, 85, 85, 85, 85, 85, 85, 85, 85 };
            var rng = new DeterministicRandom(ints);
            var state = new GameState { Packs = 1, Premium = 0, Units = [] };
            var service = new PackOpeningService(state, Mock.Of<IGameStateSaver>(), new EventBus(), rng, Mock.Of<IToastService>(),
                Mock.Of<IUnitService>());

            var rewards = await service.OpenPackAsync(CancellationToken.None);

            Assert.Equal(0, state.Packs);
            Assert.Equal(5, rewards.Count);
            Assert.True(state.Premium > 0);
            Assert.True(rewards.All(r => r.RewardType == PackRewardType.Premium));
        }

        [Fact]
        public async Task OpenPackAsync_AllRushBoostRewards_Path()
        {
            // Force rush boost path (roll >= 90 and < 95)
            var ints = new[] { 91, 91, 91, 91, 91 };
            var rng = new DeterministicRandom(ints);
            var state = new GameState { Packs = 1, RushBoosts = 0, Units = [] };
            var service = new PackOpeningService(state, Mock.Of<IGameStateSaver>(), new EventBus(), rng, Mock.Of<IToastService>(),
                Mock.Of<IUnitService>());

            var rewards = await service.OpenPackAsync(CancellationToken.None);

            Assert.Equal(0, state.Packs);
            Assert.Equal(5, rewards.Count);
            Assert.True(state.RushBoosts > 0);
            Assert.True(rewards.All(r => r.RewardType == PackRewardType.RushBoost));
        }

        [Fact]
        public async Task OpenPackAsync_AllXpBoostRewards_Path()
        {
            // Force xp boost path (roll >= 95 and < 100)
            var ints = new[] { 99, 99, 99, 99, 99 };
            var rng = new DeterministicRandom(ints);
            var state = new GameState { Packs = 1, XpBoosts = 0, Units = [] };
            var service = new PackOpeningService(state, Mock.Of<IGameStateSaver>(), new EventBus(), rng, Mock.Of<IToastService>(),
                Mock.Of<IUnitService>());

            var rewards = await service.OpenPackAsync(CancellationToken.None);

            Assert.Equal(0, state.Packs);
            Assert.Equal(5, rewards.Count);
            Assert.True(state.XpBoosts > 0);
            Assert.True(rewards.All(r => r.RewardType == PackRewardType.XpBoost));
        }
    }

    internal class DeterministicRandom(IEnumerable<int> ints, IEnumerable<double>? doubles = null) : IRandomProvider
    {
        private readonly Queue<int> _ints = new(ints);
        private readonly Queue<double> _doubles = new(doubles ?? [0.5]);

        public int Next(int min, int max)
        {
            if (_ints.Count == 0) return min;
            return _ints.Dequeue();
        }

        public double NextDouble()
        {
            if (_doubles.Count == 0) return 0.5;
            return _doubles.Dequeue();
        }
    }
}
