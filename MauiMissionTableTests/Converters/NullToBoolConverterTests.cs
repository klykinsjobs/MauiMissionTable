using MauiMissionTable.Converters;

namespace MauiMissionTableTests.Converters
{
    public class NullToBoolConverterTests
    {
        [Fact]
        public void Convert_Null_ReturnsTrue()
        {
            var converter = new NullToBoolConverter();

            var result = converter.Convert(null, null, null, null);

            Assert.True((bool)result);
        }

        [Fact]
        public void Convert_NonNull_ReturnsFalse()
        {
            var converter = new NullToBoolConverter();

            var result = converter.Convert("x", null, null, null);

            Assert.False((bool)result);
        }

        [Fact]
        public void ConvertBack_Throws()
        {
            var converter = new NullToBoolConverter();

            Assert.Throws<NotImplementedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
