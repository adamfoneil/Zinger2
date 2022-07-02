using System.Windows;
using Zinger2.ViewModels;

namespace Zinger2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
        }
    }
}
