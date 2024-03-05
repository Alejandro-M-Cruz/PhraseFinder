using System.Globalization;
using System.Windows.Data;
using PhraseFinder.Domain.Extensions;

namespace PhraseFinder.WPF.Converters;

internal class EnumDisplayNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            return enumValue.GetDisplayName();
        }
        return value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}