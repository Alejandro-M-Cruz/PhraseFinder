using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhraseFinder.Data;
using PhraseFinder.Data.Services;
using PhraseFinder.WPF.ViewModels;

namespace PhraseFinder.WPF.Extensions;

public static class ApplicationBuilderExtensions
{
    private static void AddDbContext(this HostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PhraseFinderDbContext>(optionsBuilder =>
        {
            const Environment.SpecialFolder localAppDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string dbDirectory = Path.Join(Environment.GetFolderPath(localAppDataFolder), "PhraseFinder");
            Directory.CreateDirectory(dbDirectory);
            string dbPath = Path.Join(dbDirectory, "PhraseFinder.accdb");
            JetOleDbDbContextOptionsBuilderExtensions.UseJetOleDb(optionsBuilder, $"Data Source={dbPath}");
            Console.WriteLine($"Database path: {dbPath}");
        });
    }

    private static void AddDbServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPhraseDictionaryService, PhraseDictionaryService>(provider =>
            new PhraseDictionaryService(provider.GetRequiredService<PhraseFinderDbContext>()));
        builder.Services.AddSingleton<IPhraseService, PhraseService>(provider =>
            new PhraseService(provider.GetRequiredService<PhraseFinderDbContext>()));
    }

    private static void AddViewModels(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MainViewModel>(provider =>
            new MainViewModel(provider.GetRequiredService<IPhraseDictionaryService>()));
    }

    private static void AddViews(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MainWindow>(provider =>
            new MainWindow { DataContext = provider.GetRequiredService<MainViewModel>() });
    }

    public static void AddServices(this HostApplicationBuilder builder)
    {
        builder.AddDbContext();
        builder.AddDbServices();
        builder.AddViewModels();
        builder.AddViews();
    }
}