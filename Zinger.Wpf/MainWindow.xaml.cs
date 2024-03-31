using ICSharpCode.AvalonEdit;
using System;
using System.Data;
using System.Windows;
using Zinger.Service.Extensions;
using Zinger.Service.Models;
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

    private void Window_Closed(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.InitializeAsync();
    }

    private async void RunQuery(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_viewModel.CurrentConnection is null) return;

            var queryProvider = _viewModel.CurrentConnection.GetQueryProvider();
            var result = await queryProvider.ExecuteAsync(new Query()
            {
                CommandType = CommandType.Text,
                Sql = _viewModel.Sql
            });

            _viewModel.QueryResult = result;
            if (result.DataSet is not null)
            {
                QueryResultTab.SelectedIndex = 0;
            }
        }
        catch (Exception exc)
        {
            MessageBox.Show(exc.Message);
        }
    }

    private void TextEditor_TextChanged(object sender, EventArgs e)
    {
        _viewModel.Sql = (sender as TextEditor)?.Text;
    }
}
