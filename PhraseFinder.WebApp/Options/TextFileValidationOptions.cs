namespace PhraseFinder.WebApp.Options;

public class TextFileValidationOptions
{ 
    public int MinContentLength { get; set; } = 3;
    public int MaxSizeKiloBytes { get; set; } = 10;
}
