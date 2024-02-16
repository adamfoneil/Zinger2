using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Zinger.Service;
using Zinger.ViewModels;

namespace Zinger;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        ServiceCollection services = new();
	    ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();        
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<ConnectionStore>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<ConnectionsWindow>();
    }
}
