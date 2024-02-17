using MySql.Data.MySqlClient;
using System.Data;
using Zinger.Service.Abstract;
using Zinger.Service.Models;

namespace Zinger.Service.QueryProviders;

public class MySqlQueryProvider(string connectionString) : QueryProvider
{
    private readonly string ConnectionString = connectionString;

    public override DatabaseType Type => DatabaseType.MySql;

    public override Dictionary<int, string> ParameterTypes => [];

    protected override IDbDataAdapter GetAdapter(IDbCommand command) => new MySqlDataAdapter(command as MySqlCommand);

    protected override IDbCommand GetCommand(IDbConnection connection, Query query) => new MySqlCommand(query.Sql, connection as MySqlConnection);
    
    protected override IDbConnection GetConnection() => new MySqlConnection(ConnectionString);    
}
