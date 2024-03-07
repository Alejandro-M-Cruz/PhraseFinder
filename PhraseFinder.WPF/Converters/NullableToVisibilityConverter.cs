using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PhraseFinder.WPF.Converters;

public class NullableToVisibilityConverter : IValueConverter
{
    public Visibility VisibilityIfNotNull { get; set; } = Visibility.Visible;
    public Visibility VisibilityIfNull { get; set; } = Visibility.Collapsed;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null ? VisibilityIfNotNull : VisibilityIfNull;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}