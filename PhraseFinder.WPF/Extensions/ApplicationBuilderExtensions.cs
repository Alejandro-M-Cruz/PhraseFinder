using System.ComponentModel;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhraseFinder.Data;
using PhraseFinder.Data.Services;
using PhraseFinder.WPF.Navigation;
using PhraseFinder.WPF.ViewModels;

namespace PhraseFinder.WPF.Extensions;

internal static class ApplicationBuilderExtensions
{
    private static void AddDbContext(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PhraseFinderDbContext>(optionsBuilder =>
        {
            const Environment.SpecialFolder localAppDataFolder = Environment.SpecialFolder.ApplicationData;
            string dbDirectory = Path.Join(Environment.GetFolderPath(localAppDataFolder), "PhraseFinder");
            Directory.CreateDirectory(dbDirectory);
            string dbPath = Path.Join(dbDirectory, "expresiones-y-locuciones.accdb");
            optionsBuilder.UseJetOleDb($"Data Source={dbPath}");
            Console.WriteLine($"Database path: {dbPath}");
        });
    }

    public static void AddDbServices(this IHostApplicationBuilder builder)
    {

        builder.Services.AddTransient<IPhraseDictionaryService, PhraseDictionaryService>();
        builder.Services.AddTransient<IPhraseService, PhraseService>();
    }

    private static void AddViewModels(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<PhraseDictionariesViewModel>();
        builder.Services.AddTransient<AddPhraseDictionaryViewModel>();
        builder.Services.AddTransient<PhrasesViewModel>();
    }

    private static void AddViews(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MainWindow>(provider =>
            new MainWindow { DataContext = provider.GetRequiredService<MainViewModel>() });
    }

    private static void AddNavigation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Func<Type, INotifyPropertyChanged>>(serviceProvider =>
        {
            return viewModelType => (INotifyPropertyChanged)serviceProvider.GetRequiredService(viewModelType);
        });
        builder.Services.AddSingleton<INavigationService, NavigationService>();
    }

    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.AddDbContext();
        builder.AddDbServices();
        builder.AddViewModels();
        builder.AddViews();
        builder.AddNavigation();
    }
}