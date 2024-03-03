using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var displayAttribute = value.GetType()
            .GetField(value.ToString())
            ?.GetCustomAttributes(typeof(DisplayAttribute), inherit: false)
            .FirstOrDefault() as DisplayAttribute;
        return displayAttribute?.Name ?? value.ToString();
    }
}