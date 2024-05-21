namespace PhraseFinder.WebApp.Options;

public class TextValidationOptions
{
    public int MinLength { get; set; } = 3;
    public int MaxLength { get; set; } = 10_000;
}