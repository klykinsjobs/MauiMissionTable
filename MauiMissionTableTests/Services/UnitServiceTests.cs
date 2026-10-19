using MauiMissionTable.Enums;
using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;
using MauiMissionTable.Services;
using Moq;

namespace MauiMissionTableTests.Services
{
    public class UnitServiceTests
    {
        [Fact]
        public async Task SellUnitAsync_RemovesUnitAndAddsGold_WhenAllowed()
        {
            var state = new GameState { Gold = 0, Units = [new Unit { Id = "u1", Title = "U1", Rarity = Rarity.Common }] };
            var mockRules = new Mock<IUnitSellingRules>();
            mockRules.Setup(r => r.CanSellUnit(It.IsAny<Unit>())).Returns(true);
            mockRules.Setup(r => r.CalculateSellValue(It.IsAny<Unit>())).Returns(5);
            var service = new UnitService(state, Mock.Of<IGameStateSaver>(), new EventBus(), Mock.Of<IToastService>(), mockRules.Object);
            var unit = state.Units[0];

            var result = await service.SellUnitAsync(unit);

            Assert.True(result);
            Assert.Empty(state.Units);
            Assert.Equal(5, state.Gold);
        }

        [Fact]
        public async Task SellUnitAsync_Fails_WhenCannotSell()
        {
            var state = new GameState { Gold = 0, Units = [new Unit { Id = "u1", Title = "U1", Rarity = Rarity.Common }] };
            var mockRules = new Mock<IUnitSellingRules>();
            mockRules.Setup(r => r.CanSellUnit(It.IsAny<Unit>())).Returns(false);
            var service = new UnitService(state, Mock.Of<IGameStateSaver>(), new EventBus(), Mock.Of<IToastService>(), mockRules.Object);
            var unit = state.Units[0];

            var result = await service.SellUnitAsync(unit);

            Assert.False(result);
            Assert.Single(state.Units);
            Assert.Equal(0, state.Gold);
        }
    }
}
