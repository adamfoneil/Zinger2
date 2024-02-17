using CommandLine;
using Zinger.Service.Models;

namespace Zinger.Tool;

internal enum Command
{
	StoreConnection,
	GenerateResultClass,
	TestConnections
}

internal class Options
{
	public Command Command { get; set; } = Command.GenerateResultClass;

	[Option]
	public DatabaseType DatabaseType { get; set; }

	[Option]
	public string ConnectionName { get; set; } = default!;

	[Option]
	public string InputFile { get; set; } = ".\\sql.qry";

	[Option]
	public string ConnectionString { get; set; } = default!;
}
