
using CommandLine;
using System.Data;
using System.Text;
using Zinger.Service;
using Zinger.Service.Abstract;
using Zinger.Service.Extensions;
using Zinger.Service.Interfaces;
using Zinger.Service.Models;
using Zinger.Service.QueryProviders;
using Zinger.Service.Static;
using Zinger.Tool;

internal class Program
{
	private static async Task Main(string[] args)
	{
		var store = new LocalConnectionStore();		

		await Parser
			.Default
			.ParseArguments<Options>(args)
			.WithParsedAsync(async (options) =>
			{
				switch (options.Command)
				{
					case Command.GenerateResultClass:
						await GenerateResultClassAsync(options, store);
						break;
					
					case Command.StoreConnection:
						await store.SaveAsync(new Connection()
						{
							Name = options.ConnectionName ?? throw new Exception("Connection name required when storing a connection"),
							ConnectionString = options.ConnectionString,
							Type = options.DatabaseType ?? throw new Exception("Database type is required when saving a connection")
						});
						break;

					case Command.TestConnections:
						await TestConnectionsAsync(store);
						break;
				}
			});
	}

    private static async Task TestConnectionsAsync(IConnectionStore store)
    {
		var allConnections = await store.GetAllAsync().ToArrayAsync();

		foreach (var connectionGrp in allConnections.GroupBy(cn => cn.Type))
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine(connectionGrp.Key);
			foreach (var connection in connectionGrp)
			{
				var (success, message) = connection.Test();
                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"- {connection.Name} opened successfully");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"- {connection.Name} failed: {message}");
                }
            }			
		}

		Console.ForegroundColor = ConsoleColor.Gray;
    }

    private static async Task GenerateResultClassAsync(Options options, LocalConnectionStore store)
	{
        if (!File.Exists(options.InputFilePath)) throw new FileNotFoundException($"File not found: {options.InputFilePath}");

        options.ConnectionName ??= PathHelper.InferConnectionName(options.InputFilePath, store.GetNames()) ?? throw new Exception("Couldn't determine the connection name");
		var connection = await store.LoadAsync(options.ConnectionName) ?? throw new Exception($"Connection name {options.ConnectionName} not found");		
		if (connection.ConnectionString is null) throw new ArgumentNullException(nameof(connection.ConnectionString));
		options.DatabaseType ??= connection.Type;

		QueryProvider queryProvider = options.DatabaseType switch
		{
			DatabaseType.SqlServer => new SqlServerQueryProvider(connection.ConnectionString),
			DatabaseType.MySql => new MySqlQueryProvider(connection.ConnectionString),
			DatabaseType.PostgreSql => new PostgreSqlQueryProvider(connection.ConnectionString),
			_ => throw new NotSupportedException()
		};

		if (queryProvider.Type != connection.Type) throw new Exception($"Can't use {queryProvider.Type} with {connection.Type}");

		var (parameters, sql) = ReadSqlFile(options.InputFilePath);

		Query query = new() 
		{ 
			Sql = sql, 
			Parameters = parameters 
		};

		var result = await queryProvider.ExecuteAsync(query);

		foreach (var csharpClass in result.ResultClasses)
		{

		}
	}

    private static (List<Query.Parameter> parameters, string sql) ReadSqlFile(string inputFilePath)
    {
        throw new NotImplementedException();
    }
}