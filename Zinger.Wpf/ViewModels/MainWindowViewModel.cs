using Zinger.Service;

namespace Zinger.ViewModels
{
	public class MainWindowViewModel
    {
        private readonly ConnectionStore _connectionStore;

        public MainWindowViewModel(ConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;
        }
    }
}
