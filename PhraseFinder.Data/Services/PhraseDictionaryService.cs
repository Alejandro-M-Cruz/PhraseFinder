using Microsoft.EntityFrameworkCore;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services.FileReaders;
using PhraseFinder.Domain.Services.PatternGenerators;

namespace PhraseFinder.Data.Services;

public class PhraseDictionaryService(PhraseFinderDbContext dbContext) : IPhraseDictionaryService
{
    public async Task<IEnumerable<PhraseDictionary>> GetPhraseDictionariesAsync()
    {
        return await dbContext.PhraseDictionaries.AsNoTracking().ToArrayAsync();
    }

    public async Task AddPhraseDictionaryAsync(PhraseDictionary phraseDictionary)
    {
        dbContext.PhraseDictionaries.Add(phraseDictionary);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddPhraseDictionaryFromFileAsync(PhraseDictionary phraseDictionary)
    {
        var dleTxtReader = PhraseDictionaryFileReaderFactory.CreateReader(
	        phraseDictionary.Format, 
            filePath: phraseDictionary.FilePath);
        var patternGenerator = PatternGeneratorFactory.CreateGenerator(phraseDictionary.Format);
        await foreach (var phraseEntry in dleTxtReader.ReadPhraseEntriesAsync())
        {
            var phrase = phraseEntry.ToPhrase();
            foreach (var p in patternGenerator.GeneratePatterns(phrase))
            {
				phraseDictionary.Phrases.Add(p);
			}
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