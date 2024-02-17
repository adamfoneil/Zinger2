
using CommandLine;
using Zinger.Service;
using Zinger.Service.Models;
using Zinger.Tool;

internal class Program
{
	private static async void Main(string[] args)
	{
		var store = new LocalConnectionStore();

		await Parser
			.Default
			.ParseArguments<Options>(args)
			.WithParsedAsync(async (options) =>
			{
				switch (options.Command)
				{
					case Command.StoreConnection:
						await store.SaveAsync(new Connection()
						{
							Name = options.ConnectionName,
							ConnectionString = options.ConnectionString,
							Type = options.DatabaseType
						});
						break;

					case Command.GenerateResultClass:
						break;

					case Command.TestConnections:
						break;
				}
			});
	}
}