using System.Globalization;

namespace MauiMissionTable.Converters
{
    public class GreaterThanZeroConverter : IValueConverter
    {
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not int i)
                return false;

            return i > 0;
        }

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
            => throw new NotImplementedException();
    }
}
