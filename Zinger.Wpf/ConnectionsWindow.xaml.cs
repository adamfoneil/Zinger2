﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Zinger.Service.Extensions;
using Zinger.Service.Interfaces;
using Zinger.Service.Models;
using Zinger.Service.Static;

namespace Zinger;

/// <summary>
/// Interaction logic for Connections.xaml
/// </summary>
public partial class ConnectionsWindow : Window
{
    private readonly IConnectionStore _connectionStore;

    public ConnectionsWindow(IConnectionStore connectionStore)
    {
        DataContext = this;

        _connectionStore = connectionStore;

        SavedConnections = [];
        ConnectionTypes = EnumHelper.ToDictionary<DatabaseType>();

        InitializeComponent();
        ConnectionTypeDropdown.ItemsSource = ConnectionTypes;
    }

    public ObservableCollection<Connection> SavedConnections { get; set; }
    public Dictionary<DatabaseType, string> ConnectionTypes { get; }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var list = await _connectionStore.GetAllAsync().ToListAsync();
        foreach (var item in list) SavedConnections.Add(item);
    }

    private async void TestConnections(object sender, RoutedEventArgs e)
    {
        await foreach (var item in _connectionStore.GetAllAsync())
        {
            var (result, message) = item.Test();
        }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

	private async void SaveConnections(object sender, RoutedEventArgs e)
	{
        foreach (var cn in SavedConnections) await _connectionStore.SaveAsync(cn);
	}
}
