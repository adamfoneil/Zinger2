using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Zinger2.Service;

namespace Zinger2.BlazorWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
            serviceCollection.AddSingleton<ConnectionStore>();
            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }
    }
}
