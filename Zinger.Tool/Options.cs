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
	[Option]
	public Command Command { get; set; } = Command.GenerateResultClass;

	/// <summary>
	/// this can be inferred from the .qry file folder path
	/// </summary>
	[Option]
	public DatabaseType? DatabaseType { get; set; }

	/// <summary>
	/// this can be inferred from the .qry file path
	/// </summary>
	[Option]
	public string? ConnectionName { get; set; } = default!;

	[Option]
	public string InputFile { get; set; } = ".\\sql.qry";

	public string InputFilePath => Path.GetFullPath(InputFile);

	[Option]
	public string ConnectionString { get; set; } = default!;
}
