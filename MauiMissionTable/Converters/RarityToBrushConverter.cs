using MauiMissionTable.Enums;
using System.Globalization;

namespace MauiMissionTable.Converters
{
    public class RarityToBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not Rarity rarity)
                return Colors.Transparent;

            return rarity switch
            {
                Rarity.Common => new SolidColorBrush(Color.FromArgb("#B0B0B0")),

                Rarity.Uncommon => new LinearGradientBrush(
                    [new GradientStop(Color.FromArgb("#4CAF50"), 0), new GradientStop(Color.FromArgb("#81C784"), 1)],
                    new Point(0, 0),
                    new Point(1, 1)),

                Rarity.Rare => new LinearGradientBrush(
                    [new GradientStop(Color.FromArgb("#2196F3"), 0), new GradientStop(Color.FromArgb("#64B5F6"), 1)],
                    new Point(0, 0),
                    new Point(1, 1)),

                Rarity.Epic => new LinearGradientBrush(
                    [new GradientStop(Color.FromArgb("#9C27B0"), 0), new GradientStop(Color.FromArgb("#CE93D8"), 1)],
                    new Point(0, 0),
                    new Point(1, 1)),

                Rarity.Legendary => new LinearGradientBrush(
                    [new GradientStop(Color.FromArgb("#FF9800"), 0), new GradientStop(Color.FromArgb("#FFE082"), 1)],
                    new Point(0, 0),
                    new Point(1, 1)),

                _ => Colors.Transparent
            };
        }

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
            => throw new NotImplementedException();
    }
}
