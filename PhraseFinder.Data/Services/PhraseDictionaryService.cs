using Microsoft.EntityFrameworkCore;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;

namespace PhraseFinder.Data.Services;

public class PhraseDictionaryService(PhraseFinderDbContext dbContext) : IPhraseDictionaryService
{
    public async Task<IEnumerable<PhraseDictionary>> GetPhraseDictionariesAsync()
    {
        return await dbContext.PhraseDictionaries.ToListAsync();
    }

    public async Task AddPhraseDictionaryAsync(PhraseDictionary phraseDictionary)
    {
        dbContext.PhraseDictionaries.Add(phraseDictionary);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddPhraseDictionaryFromFileAsync(PhraseDictionary phraseDictionary)
    {
        var dleTxtReader = PhraseDictionaryFileReaderFactory.CreateReader(
            PhraseDictionaryFormat.DleTxt, 
            filePath: phraseDictionary.FilePath);
        await foreach (var phraseEntry in dleTxtReader.ReadPhraseEntriesAsync())
        {
            phraseDictionary.Phrases.Add(phraseEntry.ToPhrase());
        }
        dbContext.PhraseDictionaries.Add(phraseDictionary);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdatePhraseDictionaryAsync(PhraseDictionary phraseDictionary)
    {
        dbContext.PhraseDictionaries.Update(phraseDictionary);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task DeletePhraseDictionaryAsync(PhraseDictionary phraseDictionary)
    {
        dbContext.PhraseDictionaries.Remove(phraseDictionary);
        await dbContext.SaveChangesAsync();
    }
}