using Microsoft.Data.SqlClient;
using System.Data;
using Zinger.Service.Abstract;
using Zinger.Service.Models;
using Zinger.Service.Static;

namespace Zinger.Service;

public class SqlServerQueryProvider : QueryProvider
{
	private readonly string _connectionString;

	public SqlServerQueryProvider(string connectionString)
	{
		_connectionString = connectionString;
	}

	public override Dictionary<int, string> ParameterTypes => EnumHelper.ToDictionary<SqlDbType>();

	protected override IDbDataAdapter GetAdapter(IDbCommand command) => new SqlDataAdapter(command as SqlCommand);

	protected override IDbCommand GetCommand(IDbConnection connection, Query query) => new SqlCommand(query.Sql, connection as SqlConnection);

	protected override IDbConnection GetConnection() => new SqlConnection(_connectionString);

	protected override void SetParamProperties(IDbDataParameter dbParam, Query.Parameter queryParam)
	{
		if (dbParam is SqlParameter sqlParam)
		{
			sqlParam.SqlDbType = (SqlDbType)queryParam.Type;
		}
	}
}
