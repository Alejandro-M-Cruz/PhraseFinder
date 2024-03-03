using System.Windows.Markup;

namespace PhraseFinder.WPF.Extensions;

public class EnumValuesExtension : MarkupExtension
{
    public Type? EnumType { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (EnumType == null)
        {
            throw new InvalidOperationException("The EnumType must be specified");
        }
        return GetEnumValues(EnumType);
    }

    private static IEnumerable<object> GetEnumValues(Type enumType)
    {
        return Enum.GetValues(enumType).Cast<object>();
    }
}