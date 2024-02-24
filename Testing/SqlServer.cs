using Dapper;
using SqlServer.LocalDb;
using System.Data;
using System.Reflection;
using Zinger.Service.Models;
using Zinger.Service.QueryProviders;

namespace Testing;

[TestClass]
public class SqlServer
{
    private static string GetContent(string resourceName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource not found: {resourceName}");
        var result = new StreamReader(stream).ReadToEnd();
        return result;
    }

    [TestMethod]
    public async Task SimpleQueryNoParams()
    {
        var qp = new SqlServerQueryProvider(LocalDb.GetConnectionString("master"));
        var result = await qp.ExecuteAsync(new Query()
        {
		    ResultClassNames = ["TableResult", "ColumnResult"], 
            CommandType = CommandType.Text,
            Sql = "SELECT * FROM [sys].[tables]; SELECT * FROM [sys].[columns]"
        });

        Assert.IsTrue(result.DataSet.Tables.Count == 2);
        Assert.IsTrue(result.ResultClasses.Count == 2);
        Assert.IsTrue(result.ResultClasses[0].Equals(GetContent("Testing.Resources.SqlServer.TableResult.txt")));
        Assert.IsTrue(result.ResultClasses[1].Equals(GetContent("Testing.Resources.SqlServer.ColumnResult.txt")));
    }

    [TestMethod]
    public async Task SimpleQueryWithParams()
    {
        var qp = new SqlServerQueryProvider(LocalDb.GetConnectionString("master"));
        var result = await qp.ExecuteAsync(new Query()
        {
            CommandType = CommandType.Text,
            Sql = "SELECT * FROM [sys].[tables] WHERE [name] LIKE CONCAT('%', @name, '%')",
            Parameters = [ new() { Name = "name", Type = "String", Value = "de"} ]
        });

        Assert.IsTrue(result.DataSet.Tables[0].Rows.Count == 1);
        Assert.IsTrue(result.DataSet.Tables[0].AsEnumerable().All(row => (row.Field<string>("name") ?? string.Empty).Contains("e")));
	    Assert.IsTrue(result.ResultClasses[0].Equals(GetContent("Testing.Resources.SqlServer.Result1.txt")));
    }

    [TestMethod]
    public async Task QueryWithTableParam()
    {
	    using var cn = LocalDb.GetConnection("ZingerSample");

	    await cn.ExecuteAsync(
		    @"DROP TYPE IF EXISTS [dbo].[IdList];
			CREATE TYPE [dbo].[IdList] AS TABLE (
				[Id] int NOT NULL PRIMARY KEY
			)");
    }
}