using System.Data;
using System.Diagnostics;
using Zinger2.Service.Models;

namespace Zinger2.Service.Abstract
{
    public abstract class QueryProvider
    {
        public ConnectionType Type { get; }

        protected abstract IDbConnection GetConnection();
        protected abstract IDbCommand GetCommand(IDbConnection connection, Query query);
        /// <summary>
        /// should be enum values for System.Data.DbType or SqlDbType.
        /// This allows SQL Server to use the "Structured" type which doesn't exist for other providers
        /// </summary>
        public abstract Dictionary<int, string> ParameterTypes { get; }
        protected abstract IDbDataAdapter GetAdapter(IDbCommand command);
        
        protected virtual void SetParamProperties(IDbDataParameter dbParam, Query.Parameter queryParam)
        {
            // by default, the param type is an int cast of the param type,
            // but this allows SQL Server to behave a little differently
            dbParam.DbType = (DbType)queryParam.Type;
        }

        public (bool result, string? message) TestConnection()
        {
            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();
                    return (true, null);
                }
            }
            catch (Exception exc)
            {
                return (false, exc.Message);
            }
        }

        public async Task<ExecuteResult> ExecuteAsync(Query query)
        {
            var dataSet = new DataSet();

            using var cn = GetConnection();
            cn.Open();

            using var cmd = GetCommand(cn, query);

            foreach (var p in query.Parameters)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = p.Name;               
                param.Value = p.Value ?? DBNull.Value;
                SetParamProperties(param, p);
                cmd.Parameters.Add(param);
            }

            var adapter = GetAdapter(cmd);

            var sw = Stopwatch.StartNew();
            await Task.Run(() => adapter.Fill(dataSet));
            sw.Stop();

            List<DataTable> schemaTables = new();
            using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
            do
            {
                var table = reader.GetSchemaTable();
                if (table is not null) schemaTables.Add(table);
            } while (reader.NextResult());

            return new ExecuteResult()
            {
                Elapsed = sw.Elapsed,
                DataSet = dataSet,
                SchemaTables = schemaTables
            };
        }

        protected Dictionary<int, string> EnumToDictionary<TEnum>() where TEnum : struct, Enum
        {
            var names = Enum.GetNames<TEnum>();
            var values = Enum.GetValues<TEnum>();
            return values.Select((val, index) => new
            {
                Name = names[index],
                Value = Convert.ToInt32(val)
            }).ToDictionary(item => item.Value, item => item.Name);
        }

        public class ExecuteResult
        {
            public TimeSpan Elapsed { get; init; }
            public DataSet DataSet { get; init; } = new();
            public List<DataTable> SchemaTables { get; init; } = new();
        }
    }
}