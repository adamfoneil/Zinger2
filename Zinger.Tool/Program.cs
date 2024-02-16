
using CommandLine;
using Zinger.Tool;

internal class Program
{
	private static async void Main(string[] args)
	{
		await Parser
			.Default
			.ParseArguments<Options>(args)
			.WithParsedAsync(async (options) =>
			{
				switch (options.Command)
				{
					case Command.StoreConnection:
						await StoreConnectionAsync(options.ConnectionName, options.ConnectionString);
						break;

					case Command.GenerateResultClass:
						break;
				}
			});
	}

	private static async Task StoreConnectionAsync(string connectionName, string connectionString)
	{
		throw new NotImplementedException();
	}
}