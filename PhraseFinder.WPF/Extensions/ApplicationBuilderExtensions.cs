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
            optionsBuilder.UseJetOleDb($"Data Source={dbPath}");
            Console.WriteLine($"Database path: {dbPath}");
        });
    }

    private static void AddDbServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPhraseDictionaryService, PhraseDictionaryService>();
        builder.Services.AddSingleton<IPhraseService, PhraseService>();
    }

    private static void AddViewModels(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MainViewModel>();
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