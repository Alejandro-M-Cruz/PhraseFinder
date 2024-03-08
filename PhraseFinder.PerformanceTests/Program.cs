using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhraseFinder.Data;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;

var dleTxtReader = PhraseDictionaryFileReaderFactory.CreateReader(
    PhraseDictionaryFormat.DleTxt, 
    filePath: "D:\\Proyectos\\dotNet\\TFT\\DLE.txt");

var stopwatch = Stopwatch.StartNew();
await foreach (var phraseEntry in dleTxtReader.ReadPhraseEntriesAsync())
{}
stopwatch.Stop();
Console.WriteLine($"Elapsed time for async version: {stopwatch.ElapsedMilliseconds} ms");

/*
stopwatch.Restart();
foreach (var phraseEntry in dleTxtReader.ReadPhraseEntries())
{}
stopwatch.Stop();

Console.WriteLine($"Elapsed time for sync version: {stopwatch.ElapsedMilliseconds} ms");


var dleTxtReader2 = new DleTxtPhraseDictionaryReader(filePath: "D:\\Proyectos\\dotNet\\TFT\\DLE.txt");

stopwatch.Restart();
foreach (var phraseEntry in await dleTxtReader2.Something())
{}
stopwatch.Stop();

Console.WriteLine($"Elapsed time for Task<IEnumerable> version: {stopwatch.ElapsedMilliseconds} ms");
*/

var optionsBuilder = new DbContextOptionsBuilder<PhraseFinderDbContext>();
optionsBuilder.UseJetOleDb("Data Source=PhraseFinder.accdb");
var dbContext = new PhraseFinderDbContext(optionsBuilder.Options);
dbContext.Database.Migrate();
dbContext.PhraseDictionaries.RemoveRange(dbContext.PhraseDictionaries);

/*stopwatch.Restart();
var phraseDictionary = new PhraseDictionary
{
    Name = "Dle",
    Format = PhraseDictionaryFormat.DleTxt,
    Path = "D:\\Proyectos\\dotNet\\TFT\\DLE.txt"
};
dbContext.PhraseDictionaries.Add(phraseDictionary);
await dbContext.SaveChangesAsync();
/*await foreach (var phrase in dleTxtReader.ReadPhraseEntriesAsync())
{
    phrase.PhraseDictionaryId = phraseDictionary.PhraseDictionaryId;
    dbContext.Phrases.Add(phrase);
}#1#
await dbContext.Phrases.AddRangeAsync(
    dleTxtReader.ReadPhraseEntries()
        .Select(p =>
        {
            p.PhraseDictionaryId = phraseDictionary.PhraseDictionaryId;
            return p;
        }));

await dbContext.SaveChangesAsync();

stopwatch.Stop();

Console.WriteLine("Phrase dictionary added to database");
Console.WriteLine($"{phraseDictionary.Phrases.Count} phrases where found");
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");*/

stopwatch.Restart();

var phrases = dleTxtReader.ReadPhraseEntries().ToList();

stopwatch.Stop();

Console.WriteLine($"Elapsed time for sync version: {stopwatch.ElapsedMilliseconds} ms");


stopwatch.Restart();
var phraseDictionary = new PhraseDictionary
{
    Name = "Dle",
    Format = PhraseDictionaryFormat.DleTxt,
    FilePath = "D:\\Proyectos\\dotNet\\TFT\\DLE.txt",
};
dbContext.PhraseDictionaries.Add(phraseDictionary);
await dbContext.SaveChangesAsync();
/*await foreach (var phrase in dleTxtReader.ReadPhraseEntriesAsync())
{
    phrase.PhraseDictionaryId = phraseDictionary.PhraseDictionaryId;
    dbContext.Phrases.Add(phrase);
}*/

await dbContext.SaveChangesAsync();

stopwatch.Stop();

Console.WriteLine("Phrase dictionary added to database");
Console.WriteLine($"{phraseDictionary.Phrases.Count} phrases where found");
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");