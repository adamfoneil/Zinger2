using Zinger.Service.Abstract;
using Zinger.Service.Models;
using Zinger.Service.QueryProviders;

namespace Zinger.Service.Extensions;

public static class ConnectionExtensions
{
	public static (bool Result, string? Message) TestConnection(this Connection connection)
	{
		QueryProvider queryProvider = connection.Type switch
		{
			DatabaseType.SqlServer => new SqlServerQueryProvider(connection.ConnectionString!),
			DatabaseType.MySql => new MySqlQueryProvider(connection.ConnectionString!),
			DatabaseType.PostgreSql => new PostgreSqlQueryProvider(connection.ConnectionString!),
			_ => throw new NotSupportedException()
		};

		return queryProvider.TestConnection();
	}
}
