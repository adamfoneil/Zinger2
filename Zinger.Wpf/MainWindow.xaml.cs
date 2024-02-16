using System.Windows;
using Zinger.ViewModels;

namespace Zinger
{
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
            _connectionsWin = connectionsWin;

            //InitializeComponent();
        }

        private void mnuFileConnections(object sender, RoutedEventArgs e)
        {
            _connectionsWin.Show();
        }
    }
}
