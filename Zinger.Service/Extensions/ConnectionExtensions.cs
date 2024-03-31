using Zinger.Service.Abstract;
using Zinger.Service.Models;
using Zinger.Service.QueryProviders;

namespace Zinger.Service.Extensions;

public static class ConnectionExtensions
{
    public static QueryProvider GetQueryProvider(this Connection connection)
    {
        var connectionString = connection.ConnectionString ?? throw new ArgumentException("Connection string is required");

        return connection.Type switch
        {
            DatabaseType.SqlServer => new SqlServerQueryProvider(connectionString),
            DatabaseType.MySql => new MySqlQueryProvider(connectionString),
            DatabaseType.PostgreSql => new PostgreSqlQueryProvider(connectionString),
            _ => throw new NotSupportedException()
        };
    }

    public static (bool Result, string? Message) Test(this Connection connection)
    {
        QueryProvider queryProvider = GetQueryProvider(connection);
        return queryProvider.TestConnection();
    }
}
