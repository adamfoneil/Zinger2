using System.Data;
using System.Diagnostics;
using Zinger.Service.Internal;
using Zinger.Service.Models;

namespace Zinger.Service.Abstract
{
    public abstract class QueryProvider
    {
        public abstract DatabaseType Type { get; }

        protected abstract IDbConnection GetConnection();
        protected abstract IDbCommand GetCommand(IDbConnection connection, Query query);
        /// <summary>
        /// should be enum values for System.Data.DbType or SqlDbType.
        /// This allows SQL Server to use the "Structured" type which doesn't exist for other providers
        /// </summary>
        public abstract Dictionary<int, string> ParameterTypes { get; }
        protected abstract IDbDataAdapter GetAdapter(IDbCommand command);

        public (bool result, string? message) TestConnection()
        {
            try
            {
                using var cn = GetConnection();
                cn.Open();
                return (true, null);
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
                var type = Enum.Parse<DbType>(p.Type ?? throw new Exception("Parameter type is required"));
                var param = cmd.CreateParameter();
                param.ParameterName = p.Name;
                param.DbType = type;
                param.Value = ConvertParamValue(p.Value, type) ?? DBNull.Value;
                cmd.Parameters.Add(param);
            }

            var adapter = GetAdapter(cmd);

            var sw = Stopwatch.StartNew();
            await Task.Run(() => adapter.Fill(dataSet));
            sw.Stop();

            Dictionary<string, string> resultClasses = [];
            int nameIndex = 0;
            using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
            do
            {
                var resultClassName = query.ResultClassNames.Count > nameIndex ? query.ResultClassNames[nameIndex] : $"Result{nameIndex + 1}";
                var table = reader.GetSchemaTable();
                if (table is not null) resultClasses.Add(dataSet.Tables[nameIndex].TableName, CSharpClassBuilder.FromSchemaTable(resultClassName, table));
                nameIndex++;
            } while (reader.NextResult());

            return new ExecuteResult()
            {
                Elapsed = sw.Elapsed,
                DataSet = dataSet,
                ResultClasses = resultClasses
            };
        }

        public virtual object? ConvertParamValue(object? @object, DbType dbType) => @object;

        public class ExecuteResult
        {
            public TimeSpan Elapsed { get; init; }
            public DataSet DataSet { get; init; } = new();
            public Dictionary<string, string> ResultClasses { get; init; } = [];
            public string[] IndexedResultClasses => ResultClasses.Select((kvp, i) => kvp.Value).ToArray();
        }
    }
}