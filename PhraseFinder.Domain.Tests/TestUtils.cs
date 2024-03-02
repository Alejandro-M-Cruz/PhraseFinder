namespace PhraseFinder.Domain.Tests;

public static class TestUtils
{
    public static async Task<string> WriteToTempFileAsync(string content)
    {
        string tempFilePath = Path.GetTempFileName();
        await using var writer = new StreamWriter(tempFilePath);
        await writer.WriteAsync(content);
        return tempFilePath;
    }
}