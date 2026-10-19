using System.Globalization;

namespace MauiMissionTable.Converters
{
    public class NotNullToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
            => value != null;

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
            => throw new NotImplementedException();
    }
}
