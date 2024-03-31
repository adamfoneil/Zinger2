using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Zinger.Service.Abstract;
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

		private QueryProvider.ExecuteResult? _queryResult;
		public QueryProvider.ExecuteResult? QueryResult
		{
			get => _queryResult;
			set
			{
				_queryResult = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueryResult)));
			}
		}

		public async Task InitializeAsync()
		{
			await foreach (var connection in ConnectionStore.GetAllAsync()) Connections.Add(connection);
		}

		public ObservableCollection<Connection> Connections { get; private set; } = [];
	}
}
