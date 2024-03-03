using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Models;

public enum PhraseDictionaryFormat
{
    [Display(Name = "DLE en texto plano")] 
    DleTxt
}

public static class PhraseDictionaryFormatExtensions
{
    public static string GetDisplayName(this PhraseDictionaryFormat format)
    {
        var displayAttribute = format.GetType()
            .GetField(format.ToString())
            ?.GetCustomAttributes(typeof(DisplayAttribute), inherit: false)
            .FirstOrDefault() as DisplayAttribute;
        return displayAttribute?.Name ?? format.ToString();
    }
}


