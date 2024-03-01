using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhraseFinder.Data;

namespace PhraseFinder.WPF;

public partial class App : Application
{
    private readonly IHost _host = CreateApplicationBuilder().Build();
    
    protected override async void OnStartup(StartupEventArgs eventArgs)
    {
        _host.Start();
        var dbContext = _host.Services.GetRequiredService<PhraseFinderDbContext>();
        await dbContext.Database.MigrateAsync();
        Window mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(eventArgs);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }

    private static HostApplicationBuilder CreateApplicationBuilder()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.Services.AddDbContext<PhraseFinderDbContext>(optionsBuilder =>
        {
            const Environment.SpecialFolder localAppDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string dbDirectory = Path.Join(Environment.GetFolderPath(localAppDataFolder), "PhraseFinder");
            Directory.CreateDirectory(dbDirectory);
            string dbPath = Path.Join(dbDirectory, "PhraseFinder.accdb");
            optionsBuilder.UseJetOleDb($"Data Source={dbPath}");
            Console.WriteLine($"Database path: {dbPath}");
        });
        builder.Services.AddSingleton<MainWindow>(provider =>
            new MainWindow(provider.GetRequiredService<PhraseFinderDbContext>()));

        return builder;
    }
}