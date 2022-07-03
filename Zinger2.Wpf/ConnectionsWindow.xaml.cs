using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Zinger2.Service;
using Zinger2.Service.Models;
using Zinger2.Service.Static;

namespace Zinger2
{
    /// <summary>
    /// Interaction logic for Connections.xaml
    /// </summary>
    public partial class ConnectionsWindow : Window
    {
        private readonly ConnectionStore _connectionStore;

        public ConnectionsWindow(ConnectionStore connectionStore)
        {
            DataContext = this;

            _connectionStore = connectionStore;

            SavedConnections = new ObservableCollection<Connection>();
            ConnectionTypes = EnumHelper.ToDictionary<ConnectionType>();

            InitializeComponent();
            ConnectionTypeDropdown.ItemsSource = ConnectionTypes;            
        }

        public ObservableCollection<Connection> SavedConnections { get; set; }
        public Dictionary<int, string> ConnectionTypes { get; }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = await _connectionStore.GetConnectionsAsync();
            foreach (var item in list) SavedConnections.Add(item);            
        }
    }
}
