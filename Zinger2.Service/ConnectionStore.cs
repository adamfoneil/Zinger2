using System.Text.Json;
using Zinger2.Service.Internal;
using Zinger2.Service.Models;
using static System.Environment;

namespace Zinger2.Service
{
    /// <summary>
    /// stores and retrieves saved connections, encrypting connection strings with DPAPI
    /// </summary>
    public class ConnectionStore
    {
        private readonly string _basePath;

        private const string Filename = "Connections.json";

        public ConnectionStore(string basePath)
        {
            _basePath = basePath;
        }

        public ConnectionStore() : this(DefaultBasePath)
        {
        }

        public async Task<IEnumerable<Connection>> GetConnectionsAsync()
        {
            var file = Path.Combine(_basePath, Filename);
            if (!File.Exists(file)) return Enumerable.Empty<Connection>();

            var json = await File.ReadAllTextAsync(file);

            var results = JsonSerializer.Deserialize<Connection[]>(json);
            if (results is null) return Enumerable.Empty<Connection>();

            foreach (var c in results)
            {
                c.ConnectionString = DataProtection.Decrypt(c.ConnectionString ?? string.Empty);
            }

            return results;
        }

        public async Task SaveConnectionsAsync(IEnumerable<Connection> connections)
        {
            if (!Directory.Exists(_basePath)) Directory.CreateDirectory(_basePath);

            var copy = connections.Select(c => new Connection()
            {
                Name = c.Name,
                Type = c.Type,
                ConnectionString = DataProtection.Encrypt(c.ConnectionString)
            });

            var file = Path.Combine(_basePath, Filename);
            var json = JsonSerializer.Serialize(copy, options: new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(file, json);
        }

        public static string DefaultBasePath => Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), "Zinger");
    }
}
