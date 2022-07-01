namespace Zinger2.Service.Models
{
    public enum ConnectionType
    {
        SqlServer,
        MySql,
        OleDb,
        SqlLite
    }

    public class Connection
    {
        public string? Name { get; set; }
        public ConnectionType Type { get; set; }
        public string? ConnectionString { get; set; }
    }
}
