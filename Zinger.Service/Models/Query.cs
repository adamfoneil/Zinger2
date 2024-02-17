using System.Data;

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
}
