
using CommandLine;
using Zinger.Service;
using Zinger.Service.Abstract;
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
							Name = options.ConnectionName,
							ConnectionString = options.ConnectionString,
							Type = options.DatabaseType ?? throw new Exception("Database type is required when saving a connection")
						});
						break;

					case Command.TestConnections:
						break;
				}
			});
	}

	private static async Task GenerateResultClassAsync(Options options, LocalConnectionStore store)
	{
		options.ConnectionName ??= PathHelper.InferConnectionName(options.InputFilePath, store.GetNames()) ?? throw new Exception("Couldn't determine the connection name");
		var connection = await store.LoadAsync(options.ConnectionName) ?? throw new Exception($"Connection name {options.ConnectionName} not found");
		
		if (connection.ConnectionString is null) throw new ArgumentNullException(nameof(connection.ConnectionString));
		if (!File.Exists(options.InputFilePath)) throw new FileNotFoundException($"File not found: {options.InputFilePath}");
		options.DatabaseType ??= PathHelper.InferDatabaseType(options.InputFilePath) ?? throw new Exception("Couldn't determine database type.");

		QueryProvider queryProvider = options.DatabaseType switch
		{
			DatabaseType.SqlServer => new SqlServerQueryProvider(connection.ConnectionString),
			DatabaseType.MySql => new MySqlQueryProvider(connection.ConnectionString),
			DatabaseType.PostgreSql => new PostgreSqlQueryProvider(connection.ConnectionString),
			_ => throw new NotSupportedException()
		};

		if (queryProvider.Type != connection.Type) throw new Exception($"Can't use {queryProvider.Type} with {connection.Type}");

		var query = BuildQuery(options);
		var result = await queryProvider.ExecuteAsync(query);

		foreach (var csharpClass in result.ResultClasses)
		{

		}
	}

	private static Query BuildQuery(Options options)
	{
		throw new NotImplementedException();
	}
}