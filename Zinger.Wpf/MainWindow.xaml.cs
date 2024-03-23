using System.Windows;
using Zinger.ViewModels;

namespace Zinger;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;
    private readonly ConnectionsWindow _connectionsWin;        

    public MainWindow(MainWindowViewModel viewModel, ConnectionsWindow connectionsWin)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        _connectionsWin = connectionsWin;

        InitializeComponent();
    }

    private void MnuFileConnections(object sender, RoutedEventArgs e)
    {
        _connectionsWin.Show();
    }	

	private void MnuFileExit(object sender, RoutedEventArgs e)
	{
		Application.Current.Shutdown();
	}

	private void Window_Closed(object sender, System.EventArgs e)
	{
        Application.Current.Shutdown();
	}

	private async void Window_Loaded(object sender, RoutedEventArgs e)
	{
        await _viewModel.InitializeAsync();
	}
}
