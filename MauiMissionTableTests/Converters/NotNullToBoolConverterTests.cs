using MauiMissionTable.Converters;

namespace MauiMissionTableTests.Converters
{
    public class NotNullToBoolConverterTests
    {
        [Fact]
        public void Convert_Null_ReturnsFalse()
        {
            var converter = new NotNullToBoolConverter();

            var result = converter.Convert(null, null, null, null);

            Assert.False((bool)result);
        }

        [Fact]
        public void Convert_NonNull_ReturnsTrue()
        {
            var converter = new NotNullToBoolConverter();

            var result = converter.Convert("x", null, null, null);

            Assert.True((bool)result);
        }

        [Fact]
        public void ConvertBack_Throws()
        {
            var converter = new NotNullToBoolConverter();

            Assert.Throws<NotImplementedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
