using Microsoft.Data.SqlClient;
using System.Data;
using Zinger.Service.Abstract;
using Zinger.Service.Models;

namespace Zinger.Service.QueryProviders;

public class SqlServerQueryProvider(string connectionString) : QueryProvider
{
    private readonly string ConnectionString = connectionString;

    public override DatabaseType Type => DatabaseType.SqlServer;

    public override Dictionary<int, string> ParameterTypes => [];

    protected override IDbDataAdapter GetAdapter(IDbCommand command) => new SqlDataAdapter(command as SqlCommand);

    protected override IDbCommand GetCommand(IDbConnection connection, Query query) => new SqlCommand(query.Sql, connection as SqlConnection);

    protected override IDbConnection GetConnection() => new SqlConnection(ConnectionString);
}
