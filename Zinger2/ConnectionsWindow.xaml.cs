using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
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
            _connectionStore = connectionStore;

            SavedConnections = new ObservableCollection<Connection>();
            ConnectionTypes = EnumHelper.ToObservableCollection<ConnectionType>();

            InitializeComponent();            
        }

        public ObservableCollection<Connection> SavedConnections { get; }
        public ObservableCollection<KeyValuePair<int, string>> ConnectionTypes { get; }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = await _connectionStore.GetConnectionsAsync();
            foreach (var item in list) SavedConnections.Add(item);
        }
    }
}
