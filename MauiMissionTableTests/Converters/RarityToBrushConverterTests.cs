using MauiMissionTable.Converters;
using MauiMissionTable.Enums;

namespace MauiMissionTableTests.Converters
{
    public class RarityToBrushConverterTests
    {
        [Theory]
        [InlineData(Rarity.Common)]
        [InlineData(Rarity.Uncommon)]
        [InlineData(Rarity.Rare)]
        [InlineData(Rarity.Epic)]
        [InlineData(Rarity.Legendary)]
        public void Convert_Rarity_ReturnsBrush(Rarity rarity)
        {
            var converter = new RarityToBrushConverter();

            var result = converter.Convert(rarity, null, null, null);

            Assert.IsType<Brush>(result, exactMatch: false);    // SolidColorBrush, LinearGradientBrush, and RadialGradientBrush
        }

        [Theory]
        [InlineData(Rarity.Legendary + 1)]  // Non-existant rarity
        [InlineData("abc")]
        [InlineData(null)]
        public void Convert_NonRarity_ReturnsTransparent(object? value)
        {
            var converter = new RarityToBrushConverter();

            var result = converter.Convert(value, null, null, null);

            Assert.Equal(Colors.Transparent, result);
        }

        [Fact]
        public void ConvertBack_Throws()
        {
            var converter = new RarityToBrushConverter();

            Assert.Throws<NotImplementedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
