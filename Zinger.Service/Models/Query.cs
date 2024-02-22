using System.Data;
using System.Text;

namespace Zinger.Service.Models;

public class Query
{
    public List<string> ResultClassNames { get; set; } = [];
    public CommandType CommandType { get; set; }
    public string? Sql { get; set; }
    public List<Parameter> Parameters { get; set; } = [];

    public class Parameter
    {
        public string? Name { get; set; }
        public DbType Type { get; set; }
        public object? Value { get; set; }
    }

	public static Query FromStream(Stream stream)
	{
		var (header, body) = ReadParts(stream, "---");

		return new()
		{
			Parameters = ParseParameters(header),
			Sql = body
		};
	}

	public static Query FromFile(string path)
	{
		using var stream = File.OpenRead(path);
		return FromStream(stream);
	}
    
	private static List<Parameter> ParseParameters(string paramText)
	{
		throw new NotImplementedException();
	}

	private static (string Header, string Body) ReadParts(Stream stream, string separator)
	{
		StringBuilder[] stringBuilders = [new(), new()];
		int index = 0;

		using var reader = new StreamReader(stream);
		while (!reader.EndOfStream)
		{
			var line = reader.ReadLine();
			if (line is null) continue;

			if (line.Equals(separator) && index == 0)
			{
				index++;
				continue;
			}

			stringBuilders[index].AppendLine(line);
		}

		return (stringBuilders[0].ToString(), stringBuilders[1].ToString());
	}
}
