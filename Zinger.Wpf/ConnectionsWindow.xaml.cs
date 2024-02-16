using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Zinger.Service;
using Zinger.Service.Models;
using Zinger.Service.Static;

namespace Zinger
{
	/// <summary>
	/// Interaction logic for Connections.xaml
	/// </summary>
	public partial class ConnectionsWindow : Window
    {
        private readonly LocalConnectionStore _connectionStore;

        public ConnectionsWindow(LocalConnectionStore connectionStore)
        {
            DataContext = this;

            _connectionStore = connectionStore;

            SavedConnections = new ObservableCollection<Connection>();
            ConnectionTypes = EnumHelper.ToDictionary<DatabaseType>();

            InitializeComponent();
            ConnectionTypeDropdown.ItemsSource = ConnectionTypes;            
        }

        public ObservableCollection<Connection> SavedConnections { get; set; }
        public Dictionary<int, string> ConnectionTypes { get; }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = await _connectionStore.GetAllAsync().ToListAsync();
            foreach (var item in list) SavedConnections.Add(item);            
        }
    }
}
