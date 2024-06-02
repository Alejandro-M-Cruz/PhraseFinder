using System.ComponentModel;
using System.Diagnostics;
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
            var connectionString = GetConnectionString();
            optionsBuilder.UseJetOleDb(connectionString);
            Console.WriteLine($"Connection string: {connectionString}");
            Debug.WriteLine($"Connection string: {connectionString}");
        });
    }

    public static void AddDbServices(this IHostApplicationBuilder builder)
    {

        builder.Services
	        .AddTransient<IPhraseDictionaryService, PhraseDictionaryService>()
	        .AddTransient<IPhraseService, PhraseService>();
    }

    private static void AddViewModels(this IHostApplicationBuilder builder)
    {
        builder.Services
	        .AddSingleton<MainViewModel>()
            .AddTransient<PhraseDictionariesViewModel>()
            .AddTransient<AddPhraseDictionaryViewModel>()
            .AddTransient<PhrasesViewModel>();
    }

    private static void AddWindows(this IHostApplicationBuilder builder)
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
        builder.AddWindows();
        builder.AddNavigation();
    }

    private static string GetConnectionString()
    {
        var appBaseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(
            Path.DirectorySeparatorChar);
        var debugSubDir = "bin" + Path.DirectorySeparatorChar +
                          "Debug" + Path.DirectorySeparatorChar +
                          "net8.0-windows";
        if (appBaseDir.TrimEnd(Path.DirectorySeparatorChar).EndsWith(debugSubDir))
        {
            appBaseDir = appBaseDir[..^debugSubDir.Length];
        }
        var dbDir = Path.Combine(appBaseDir, "Data");
        Directory.CreateDirectory(dbDir);
        var dbPath = Path.Combine(dbDir, "PhraseFinder.accdb");
        return $"Data Source={dbPath}";
    }
}