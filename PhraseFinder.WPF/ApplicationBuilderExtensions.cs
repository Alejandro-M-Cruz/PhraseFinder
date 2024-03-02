using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhraseFinder.Data;
using PhraseFinder.Data.Services;

namespace PhraseFinder.WPF;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureDatabase(this HostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PhraseFinderDbContext>(optionsBuilder =>
        {
            const Environment.SpecialFolder localAppDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string dbDirectory = Path.Join(Environment.GetFolderPath(localAppDataFolder), "PhraseFinder");
            Directory.CreateDirectory(dbDirectory);
            string dbPath = Path.Join(dbDirectory, "PhraseFinder.accdb");
            optionsBuilder.UseJetOleDb($"Data Source={dbPath}");
            Console.WriteLine($"Database path: {dbPath}");
        });
    }
    
    public static void ConfigureViews(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MainWindow>(provider =>
            new MainWindow(provider.GetRequiredService<PhraseFinderDbContext>()));
    }
    
    public static void ConfigureServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPhraseDictionaryService, PhraseDictionaryService>(provider => 
            new PhraseDictionaryService(provider.GetRequiredService<PhraseFinderDbContext>()));
        builder.Services.AddSingleton<IPhraseService, PhraseService>(provider => 
            new PhraseService(provider.GetRequiredService<PhraseFinderDbContext>()));
    }
}