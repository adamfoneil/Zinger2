using System.Data;

namespace Zinger2.Service.Models
{
    public class Query
    {
        public CommandType CommandType { get; set; }
        public string? Sql { get; set; }
        public List<Parameter> Parameters { get; set; } = new();

        public class Parameter
        {
            public string? Name { get; set; }
            public int Type { get; set; }
            public object? Value { get; set; }
        }
    }
}
