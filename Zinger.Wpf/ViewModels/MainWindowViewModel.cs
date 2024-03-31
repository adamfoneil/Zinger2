using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Zinger.Service.Interfaces;
using Zinger.Service.Models;

namespace Zinger.ViewModels
{
	public class MainWindowViewModel(IConnectionStore connectionStore) : INotifyPropertyChanged
	{
		public IConnectionStore ConnectionStore { get; } = connectionStore;

		private Connection? _currentConnection;
		private string? _sql;

		public event PropertyChangedEventHandler? PropertyChanged;

		public Connection? CurrentConnection 
		{ 
			get => _currentConnection; 
			set
			{
				_currentConnection = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentConnection)));
			}
		}

		public string? Sql
		{
			get => _sql;
			set
			{
				_sql = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sql)));
			}
		}

		public async Task InitializeAsync()
		{
			await foreach (var connection in ConnectionStore.GetAllAsync()) Connections.Add(connection);
		}

		public ObservableCollection<Connection> Connections { get; private set; } = [];
	}
}
