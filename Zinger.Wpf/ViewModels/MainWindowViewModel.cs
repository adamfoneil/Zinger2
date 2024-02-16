using Zinger.Service;

namespace Zinger.ViewModels
{
	public class MainWindowViewModel
    {
        private readonly LocalConnectionStore _connectionStore;

        public MainWindowViewModel(LocalConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;
        }
    }
}
