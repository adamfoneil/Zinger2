using Zinger.Service.Models;
using Zinger.Service.Static;

namespace Testing;

[TestClass]
public class ToolMethods
{
    [DataRow("c:\\whatever\\MySql\\this\\query.qry", DatabaseType.MySql)]
    [DataRow("c:\\this\\that\\other\\SqlServer\\this-query.qry", DatabaseType.SqlServer)]
    [TestMethod]
    public void InferDatabaseType(string path, DatabaseType databaseType)
    {
        var type = PathHelper.InferDatabaseType(path);
        Assert.AreEqual(databaseType, type);
    }

    [DataRow("c:\\whatever\\MySql\\Hello\\query.qry", "Hello")]
    [DataRow("c:\\this\\that\\other\\SqlServer\\Wombat\\this-query.qry", "Wombat")]
    [TestMethod]
    public void InferConnectionName(string path, string connectionName)
    {
        var connectionNames = new[] { "Hello", "Wombat" };
        var name = PathHelper.InferConnectionName(path, connectionNames);
        Assert.AreEqual(name, connectionName);
    }
}
