using Zinger2.Service;

namespace Zinger2.ViewModels
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
