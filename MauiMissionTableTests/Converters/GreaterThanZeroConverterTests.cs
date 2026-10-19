using MauiMissionTable.Converters;

namespace MauiMissionTableTests.Converters
{
    public class GreaterThanZeroConverterTests
    {
        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        public void Convert_IntValues_ReturnsExpected(object value, bool expected)
        {
            var converter = new GreaterThanZeroConverter();

            var result = converter.Convert(value, null, null, null);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData(null)]
        public void Convert_NonInt_ReturnsFalse(object? value)
        {
            var converter = new GreaterThanZeroConverter();

            var result = converter.Convert(value, null, null, null);

            Assert.False((bool?)result);
        }

        [Fact]
        public void ConvertBack_Throws()
        {
            var converter = new GreaterThanZeroConverter();

            Assert.Throws<NotImplementedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
