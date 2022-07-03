using System.Text.Json.Serialization;

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

        /// <summary>
        /// this is so dropdown binding works
        /// </summary>
        [JsonIgnore]
        public int BindType
        {
            get => (int)Type;
            set => Type = (ConnectionType)value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Connection connection)
            {
                return GetHashCode().Equals(connection.GetHashCode());               
            }

            return false;
        }

        public override int GetHashCode() => (Name?.ToLower() + Type.ToString() + ConnectionString?.ToLower()).GetHashCode();
    }
}
