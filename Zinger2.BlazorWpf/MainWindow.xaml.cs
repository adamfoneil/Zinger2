using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Zinger.Service;

namespace Zinger.BlazorWpf
{
	public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
            serviceCollection.AddSingleton<ConnectionStore>();
            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }
    }
}
