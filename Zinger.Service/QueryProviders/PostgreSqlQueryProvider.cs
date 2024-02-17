using Npgsql;
using System.Data;
using Zinger.Service.Abstract;
using Zinger.Service.Models;

namespace Zinger.Service.QueryProviders;

public class PostgreSqlQueryProvider(string connectionString) : QueryProvider
{
    private readonly string ConnectionString = connectionString;

    public override DatabaseType Type => DatabaseType.PostgreSql;

    public override Dictionary<int, string> ParameterTypes => [];

    protected override IDbDataAdapter GetAdapter(IDbCommand command) => new NpgsqlDataAdapter(command as NpgsqlCommand ?? throw new Exception("missing command"));

    protected override IDbCommand GetCommand(IDbConnection connection, Query query) => new NpgsqlCommand(query.Sql, connection as NpgsqlConnection);

    protected override IDbConnection GetConnection() => new NpgsqlConnection(ConnectionString);

    /// <summary>
    /// Unlike other ADO providers, Postgres doesn't do implicit type conversions on param
    /// values to their target DbTypes. It complains if you try to assign a string to an int,
    /// for instance, so I do this explicitly.
    /// </summary>
    public override object ConvertParamValue(object @object, DbType dbType)
    {
        if (dbType == DbType.Int32) return Convert.ToInt32(@object);

        return @object;
    }
}
