using System.Globalization;
using System.Windows;
using HandyControl.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhraseFinder.Data;
using PhraseFinder.WPF.Extensions;

namespace PhraseFinder.WPF;

public partial class App : Application
{
    private readonly IHost _host = CreateApplicationBuilder().Build();

    protected override async void OnStartup(StartupEventArgs eventArgs)
    {
        ConfigHelper.Instance.SetLang("es");
        var spanish = new CultureInfo("es-ES");
        CultureInfo.DefaultThreadCurrentCulture = spanish;
        CultureInfo.DefaultThreadCurrentUICulture = spanish;
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
        builder.AddServices();
        return builder;
    }
}