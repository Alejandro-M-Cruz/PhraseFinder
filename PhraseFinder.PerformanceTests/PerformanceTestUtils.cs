using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PhraseFinder.Data;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;

namespace PhraseFinder.Domain.PerformanceTests;

public static class PerformanceTestUtils
{
    private static readonly PhraseFinderDbContext _dbContext;
    private static readonly IPhraseDictionaryFileReader _reader;

    static PerformanceTestUtils()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PhraseFinderDbContext>();
        optionsBuilder.UseJetOleDb("Data Source=PerformanceTests.accdb");
        _dbContext = new PhraseFinderDbContext(optionsBuilder.Options);
        //_dbContext.Database.EnsureDeleted();
        //_dbContext.Database.EnsureCreated();
        _reader = PhraseDictionaryFileReaderFactory.CreateReader(
            PhraseDictionaryFormat.DleTxt,
            "D:\\Proyectos\\dotNet\\TFT\\DLE.txt");
    }

    public static async void ReadDictionaryFile()
    {
        var stopwatch = Stopwatch.StartNew();
        await foreach (var phraseEntry in _reader.ReadPhraseEntriesAsync())
        { }
        stopwatch.Stop();
        Console.WriteLine($"Read dictionary file in: {stopwatch.ElapsedMilliseconds} ms");
    }

    public static async void AddPhrases()
    {
        var stopwatch = Stopwatch.StartNew();
        var phraseDictionary = new PhraseDictionary
        {
            Name = "Dle", 
            Format = PhraseDictionaryFormat.DleTxt, 
            FilePath = "D:\\Proyectos\\dotNet\\TFT\\DLE.txt" 
        };
        _dbContext.PhraseDictionaries.Add(phraseDictionary);
        await _dbContext.SaveChangesAsync();
        await foreach (var phraseEntry in _reader.ReadPhraseEntriesAsync())
        {
            var phrase = new Phrase
            {
                Name = phraseEntry.Name,
                BaseWord = phraseEntry.BaseWord,
                RegExPattern = phraseEntry.BaseWord,
                PhraseDictionaryId = phraseDictionary.PhraseDictionaryId,
                Definitions = phraseEntry.DefinitionToExamples
                    .Select(kvp => new PhraseDefinition
                    {
                        Definition = kvp.Key,
                        Examples = kvp.Value.Select(e => new PhraseExample { Example = e }).ToList()
                    }).ToList()
            };
            _dbContext.Phrases.Add(phrase);
        }
        await _dbContext.SaveChangesAsync();
        stopwatch.Stop();
        Console.WriteLine($"{_dbContext.Phrases.Count()} phrases were found");
        Console.WriteLine($"Phrase dictionary added to database in {stopwatch.ElapsedMilliseconds} ms");
    }

    public static void WriteOneWordPhrasesToCsv()
    {
        var oneWordPhrases =
            from p in _dbContext.Phrases.Include(p => p.Definitions).ToList()
            where !p.Name.Trim().Contains(' ')
            select p;

        using var writer = new StreamWriter("oneWordPhrases.csv");
        writer.WriteLine("Palabra base;Locución;Definición");
        var n = 0;
        var types = new HashSet<string>();

        foreach (var p in oneWordPhrases)
        {
            if (p == null)
            {
                continue;
            }

            if (p.Definitions.Count > 0)
            {
                n++;
            }

            foreach (var d in p.Definitions)
            {
                types.Add(d.Definition.Split('.')[1]);
                writer.WriteLine($"{p.BaseWord};{p.Name};{d.Definition}");
            }
        }

        Console.WriteLine($"Found {n} one word phrases");
        foreach (var type in types)
        {
            Console.WriteLine(type);
        }
    }

    public static void WritePhrasesThatMatchRegexToTxtFile(Regex regex, string filePath)
    {
        var stopwatch = Stopwatch.StartNew();
        var phrasesThatMatchRegex =             
            from p in _dbContext.Phrases.AsEnumerable()
            where regex.IsMatch(p.Name)
            select p;
        using var writer = new StreamWriter(filePath);
        int n = 0;
        foreach (var p in phrasesThatMatchRegex)
        {
            writer.WriteLine($"{p.Name} ({p.BaseWord})");
            n++;
        }
        stopwatch.Stop();
        Console.WriteLine($"Found {n} phrases that match regex " +
                          $"in {stopwatch.ElapsedMilliseconds} ms");
    }

    public static void RegexMatchGroupsExample()
    {
        var regex = new Regex(
            @"(\b(\w+(o|o[a-rt-zA-RT-Z]|e)),\s\b(\w{1,2}a)|\b(\w+(os|es)),\s\b(\w{1,2}as))",
            RegexOptions.Compiled);
        var phrase = "pecador, ra de mí";
        var matches = regex.Matches(phrase);
        foreach (var match in matches.AsEnumerable())
        {
            Console.WriteLine($"Match: {match.Value}");
            Console.WriteLine($"Groups: [{string.Join(";", match.Groups.Values)}]");
        }
    } 
}