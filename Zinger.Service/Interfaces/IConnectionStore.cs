﻿using Zinger.Service.Models;

namespace Zinger.Service.Interfaces;

public interface IConnectionStore
{
    Task SaveAsync(Connection connection);
    Task DeleteAsync(string name);
    Task<Connection> LoadAsync(string name);
    IAsyncEnumerable<Connection> GetAllAsync();
    string[] GetNames();
}
