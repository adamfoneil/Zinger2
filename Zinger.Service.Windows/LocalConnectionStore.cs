using System.Text.Json;
using Zinger.Service.Interfaces;
using Zinger.Service.Models;
using Zinger.Service.Windows;
using static System.Environment;

namespace Zinger.Service;

/// <summary>
/// stores and retrieves saved connections, encrypting connection strings with DPAPI
/// </summary>
public class LocalConnectionStore(string basePath) : IConnectionStore
{
    private readonly string _basePath = basePath;

    private const string Ext = ".cn";

    public LocalConnectionStore(SpecialFolder folder, string folderName) : this(Path.Combine(GetFolderPath(folder), folderName))
    {
    }

    public LocalConnectionStore() : this(DefaultBasePath)
    {
    }

    private string GetFilename(string connectionName) => Path.Combine(_basePath, $"{connectionName}{Ext}");

    public async Task SaveAsync(Connection connection)
    {
        ArgumentNullException.ThrowIfNull(nameof(connection));

        var json = JsonSerializer.Serialize(connection);
        var encrypted = DataProtection.Encrypt(json);
        var outputFile = GetFilename(connection.Name);
        var folder = Path.GetDirectoryName(outputFile) ?? throw new Exception("couldn't get folder");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        await File.WriteAllTextAsync(outputFile, encrypted);
    }

    public async Task<Connection> LoadAsync(string name)
    {
        var file = GetFilename(name);

        if (!File.Exists(file)) throw new FileNotFoundException($"File not found: {file}");

        var content = await File.ReadAllTextAsync(file);
        var json = DataProtection.Decrypt(content);
        return JsonSerializer.Deserialize<Connection>(json) ?? throw new Exception("Couldn't deserialize connection data");
    }

    public async IAsyncEnumerable<Connection> GetAllAsync()
    {
        var files = GetNames();

        foreach (var file in files)
        {
            yield return await LoadAsync(file);
        }
    }

    public string[] GetNames() =>
        Directory
            .GetFiles(_basePath, $"*{Ext}")
            .Select(fileName => Path.GetFileNameWithoutExtension(fileName))
            .ToArray();

    public static string DefaultBasePath => Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), "Zinger");
}
