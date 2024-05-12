using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PhraseFinder.Data;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services.FileReaders;

namespace PhraseFinder.Domain.PerformanceTests;

public static partial class PerformanceTestUtils
{
    public static readonly PhraseFinderDbContext DbContext;
    private static readonly IPhraseDictionaryFileReader _reader;

    static PerformanceTestUtils()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PhraseFinderDbContext>();
        optionsBuilder.UseJetOleDb("Data Source=PerformanceTests.accdb");
        DbContext = new PhraseFinderDbContext(optionsBuilder.Options);
        ResetDatabase();
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

    public static void ResetDatabase()
    {
        DbContext.Database.EnsureDeleted();
		DbContext.Database.Migrate();
	}

    public static async Task AddPhrases()
    {
        ResetDatabase();
        var stopwatch = Stopwatch.StartNew();
        var phraseDictionary = new PhraseDictionary
        {
            Name = "Dle", 
            Format = PhraseDictionaryFormat.DleTxt, 
            FilePath = "D:\\Proyectos\\dotNet\\TFT\\DLE.txt" 
        };
        DbContext.PhraseDictionaries.Add(phraseDictionary);
        await DbContext.SaveChangesAsync();
        await foreach (var phraseEntry in _reader.ReadPhraseEntriesAsync())
        {
	        var phrase = phraseEntry.ToPhrase();
            DbContext.Phrases.Add(phrase);
        }
        await DbContext.SaveChangesAsync();
        stopwatch.Stop();
        Console.WriteLine($"{DbContext.Phrases.Count()} phrases were found");
        Console.WriteLine($"Phrase dictionary added to database in {stopwatch.ElapsedMilliseconds} ms");
    }

    public static async Task AddPhrasesWithBulkInsert()
	{
		ResetDatabase();
		var stopwatch = Stopwatch.StartNew();
		var phraseDictionary = new PhraseDictionary
		{
			Name = "Dle",
			Format = PhraseDictionaryFormat.DleTxt,
			FilePath = "D:\\Proyectos\\dotNet\\TFT\\DLE.txt"
		};
		DbContext.PhraseDictionaries.Add(phraseDictionary);
		await DbContext.SaveChangesAsync();
		var phrases = new List<Phrase>();
		await foreach (var phraseEntry in _reader.ReadPhraseEntriesAsync())
		{
			var phrase = phraseEntry.ToPhrase();
			phrases.Add(phrase);
		}
		DbContext.Phrases.AddRange(phrases);
		await DbContext.SaveChangesAsync();
		stopwatch.Stop();
		Console.WriteLine($"{DbContext.Phrases.Count()} phrases were found");
		Console.WriteLine($"Phrase dictionary added to database in {stopwatch.ElapsedMilliseconds} ms");
	}

	public static void WriteOneWordPhrasesToCsv()
    {
        var oneWordPhrases =
            from p in DbContext.Phrases.Include(p => p.Definitions).ToList()
            where !p.Value.Trim().Contains(' ')
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
                writer.WriteLine($"{p.BaseWord};{p.Value};{d.Definition}");
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
            from p in DbContext.Phrases.AsEnumerable()
            where regex.IsMatch(p.Value)
            select p;
        using var writer = new StreamWriter(filePath);
        int n = 0;
        foreach (var p in phrasesThatMatchRegex)
        {
            writer.WriteLine($"{p.Value} ({p.BaseWord})");
            n++;
        }
        stopwatch.Stop();
        Console.WriteLine($"Found {n} phrases that match regex " +
                          $"in {stopwatch.ElapsedMilliseconds} ms");
    }

    public static void RegexMatchGroupsExample()
    {
        var regex = new Regex(
            @"\b\w*(\w)(o(s|)),\s\w?\1(a\3)\b|\b\w*([a-kA-Km-zM-Z])(e(s|)),\s\w?\5(a\7)\b|\b\w+(o([a-rA-Rt-zT-Z])),\s(\10(a))\b",
            RegexOptions.Compiled);
        List<string> phrases = ["alguno, na que otro, tra", "algunos, nas que otros, tras", "señores, ras"];
        foreach (var phrase in phrases)
        {
            var matches = regex.Matches(phrase);
            Console.WriteLine($"Phrase: {phrase}");
            foreach (var match in matches.AsEnumerable())
            {
                Console.WriteLine($"Match: {match.Value}");
                Console.WriteLine($"Group successes: [{string.Join(";", match.Groups.Values.Select(g => g.Success.ToString()))}]");
                Console.WriteLine($"Group indexes: [{string.Join(";", match.Groups.Values.Select(g => g.Index.ToString()))}]");
                Console.WriteLine($"Groups: [{string.Join(";", match.Groups.Values)}]");
            }
            Console.WriteLine();
        }
    }

    private static readonly Regex SampleCompiledRegex = new(
        @"(\b(\w+(o|o[a-rt-zA-RT-Z]|e)),\s\b(\w{1,2}a)|\b(\w+(os|es)),\s\b(\w{1,2}as))",
        RegexOptions.Compiled);

    [GeneratedRegex(@"(\b(\w+(o|o[a-rt-zA-RT-Z]|e)),\s\b(\w{1,2}a)|\b(\w+(os|es)),\s\b(\w{1,2}as))")]
    private static partial Regex SampleGeneratedRegex();

    public static void CompiledVsGeneratedRegex()
    {
        var phrase = "pecador, ra de mí";
        var stopwatch = Stopwatch.StartNew();
        for (var i = 0; i < 1_000_000; i++)
        {
            var match = SampleCompiledRegex.Match(phrase);
        }
        stopwatch.Stop();
        Console.WriteLine($"Compiled regex: {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Restart();
        for (var i = 0; i < 1_000_000; i++)
        {
            var match = SampleGeneratedRegex().Match(phrase);
        }
        stopwatch.Stop();
        Console.WriteLine($"Generated regex: {stopwatch.ElapsedMilliseconds} ms");
    }
}