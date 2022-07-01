﻿using Microsoft.Data.SqlClient;
using System.Data;
using Zinger2.Service.Abstract;
using Zinger2.Service.Models;

namespace Zinger2.Service
{
    public class SqlServerQueryProvider : QueryProvider
    {
        private readonly string _connectionString;

        public SqlServerQueryProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override Dictionary<int, string> ParameterTypes => EnumToDictionary<SqlDbType>();

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
}