using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;

namespace PhraseFinder.Data.Services;

public class PhraseDictionaryService(PhraseFinderDbContext dbContext) : IPhraseDictionaryService
{
    public async Task AddPhraseDictionaryAsync(string filePath)
    {
        var dleTxtReader = PhraseDictionaryReaderFactory.CreateReader(
            PhraseDictionaryFormat.DleTxt, 
            filePath: filePath);
        List<Phrase> phrases = [];
        await foreach (var phraseEntry in dleTxtReader.ReadPhraseEntriesAsync())
        {
            phrases.Add(phraseEntry.ToPhrase());
        }
        var phraseDictionary = new PhraseDictionary
        {
            Name = "New Dictionary",
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = filePath,
            Phrases = phrases
        };
        await AddPhraseDictionaryAsync(phraseDictionary);
    }

    public async Task AddPhraseDictionaryAsync(PhraseDictionary phraseDictionary)
    {
        await dbContext.PhraseDictionaries.AddAsync(phraseDictionary);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task RemovePhraseDictionaryAsync(PhraseDictionary phraseDictionary)
    {
        dbContext.PhraseDictionaries.Remove(phraseDictionary);
        await dbContext.SaveChangesAsync();
    }
}