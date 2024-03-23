using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zinger.Service.Interfaces;
using Zinger.Service.Models;

namespace Zinger.ViewModels
{
	public class MainWindowViewModel(IConnectionStore connectionStore)
	{
		public IConnectionStore ConnectionStore { get; } = connectionStore;

		public async Task InitializeAsync()
		{
			await foreach (var connection in ConnectionStore.GetAllAsync()) Connections.Add(connection);
		}

		public ObservableCollection<Connection> Connections { get; private set; } = [];
    }
}
