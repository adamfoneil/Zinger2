using System.Data;
using System.Text;
using System.Text.RegularExpressions;

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
		public string? Type { get; set; }
		public object? Value { get; set; }
	}

	public static Query FromStream(Stream stream)
	{
		var (header, body) = ReadParts(stream, "---");

		return new()
		{
			Parameters = ParseParameters(header).ToList(),
			Sql = body
		};
	}

	public static Query FromFile(string path)
	{
		using var stream = File.OpenRead(path);
		return FromStream(stream);
	}
	
	private static IEnumerable<Parameter> ParseParameters(string paramText)
	{
		var lines = paramText.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
		int index = 0;
		foreach (var line in lines)
		{
			index++;
			yield return ParseParam(line, index);
		}

		static Parameter ParseParam(string line, int index)
		{
			var nameMatch = Regex.Match(line, @"@([a-zA-Z][a-zA-Z0-9_]*)");
			var paramName = (nameMatch.Success) ? nameMatch.Value : $"param{index}";

            var typeAliases = new Dictionary<string, string>()
            {
                ["int"] = "Int32"
            };
			foreach (var alias in typeAliases)
			{
				line = line.Replace(alias.Key + " ", alias.Value + " ");
			}

            var types = Enum.GetNames<DbType>();
			
			var typeName = types
				.FirstOrDefault(type => line.Contains(type + " ")) ??
				throw new Exception("Parameter type not recognized");
			
			return new() 
			{ 
				Name = paramName, 
				Type = typeName, 
				Value = line[(line.IndexOf('=') + 1)..].Trim()
			};
		}
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
		
		// if there's no separator, we assume that all content is Body and header is empty
		return (index > 0) ? (stringBuilders[0].ToString(), stringBuilders[1].ToString()) : (string.Empty, stringBuilders[0].ToString());
	}
}
