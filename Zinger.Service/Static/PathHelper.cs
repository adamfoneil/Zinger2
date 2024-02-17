using Zinger.Service.Models;

namespace Zinger.Service.Static;

public static class PathHelper
{
    public static string? InferConnectionName(string queryFile, string[] connectionNames)
    {
        var folders = ParseFolders(queryFile);
        var inUse = connectionNames.Join(folders, c => c, f => f, (c, f) => c);
        return inUse.FirstOrDefault();
    }

    public static DatabaseType? InferDatabaseType(string queryFile)
    {
        var queryProviders = Enum.GetValues<DatabaseType>().Select(val => new { Name = val.ToString(), Value = val });
        var folders = ParseFolders(queryFile);
        var inUse = queryProviders.Join(folders, qp => qp.Name, f => f, (qp, f) => qp.Value);
        return inUse.FirstOrDefault();
    }

    private static string[] ParseFolders(string path) => path.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
}
