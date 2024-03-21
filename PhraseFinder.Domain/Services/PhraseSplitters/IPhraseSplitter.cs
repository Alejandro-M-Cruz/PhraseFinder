namespace PhraseFinder.Domain.Services.PhraseSplitters;

public interface IPhraseSplitter
{
    public string[] SplitPhrase(string phrase);
}