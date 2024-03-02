using PhraseFinder.Data.Tests.Fixtures;
using PhraseFinder.Domain.Models;
using Xunit.Abstractions;

namespace PhraseFinder.Data.Tests;

public class UnitTest1(ITestOutputHelper output)
{
    [Fact]
    public void Test1()
    {
        var fixture = new TestDatabaseFixture();
        using var context = fixture.CreateContext();
        var phraseDictionary = new PhraseDictionary
        {
            Name = "Dle",
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = "C:\\Dle.txt"
        };
        context.PhraseDictionaries.Add(phraseDictionary);
        output.WriteLine(phraseDictionary.AddedAt.ToLongTimeString());

        context.SaveChanges();

        var actualPhraseDictionary = context.PhraseDictionaries
            .OrderBy(pd => pd.Name)
            .Last(pd => pd.Name == "Dle");
        Assert.Equal("Dle", actualPhraseDictionary.Name);
        Assert.Equal(PhraseDictionaryFormat.DleTxt, actualPhraseDictionary.Format);
        Assert.Equal("C:\\Dle.txt", actualPhraseDictionary.FilePath);
        output.WriteLine(actualPhraseDictionary.AddedAt.ToLongTimeString());
    }
}