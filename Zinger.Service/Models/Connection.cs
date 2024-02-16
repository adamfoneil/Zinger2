using System.Text.Json.Serialization;

namespace Zinger.Service.Models;

public enum DatabaseType
{
    SqlServer,
    MySql,
    PostgreSql,
    OleDb,
    SqlLite
}

public class Connection
{
    public string Name { get; set; } = default!;
    public DatabaseType Type { get; set; }
    public string? ConnectionString { get; set; }

    /// <summary>
    /// this is so dropdown binding works
    /// </summary>
    [JsonIgnore]
    public int BindType
    {
        get => (int)Type;
        set => Type = (DatabaseType)value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Connection connection)
        {
            return GetHashCode().Equals(connection.GetHashCode());               
        }

        return false;
    }

	public override string ToString() => $"{Name} ({Type})";
	
	public override int GetHashCode() => (Name?.ToLower() + Type.ToString() + ConnectionString?.ToLower()).GetHashCode();
}
