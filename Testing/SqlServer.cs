using SqlServer.LocalDb;
using System.Data;
using Zinger2.Service;
using Zinger2.Service.Models;

namespace Testing
{
    [TestClass]
    public class SqlServer
    {
        [TestMethod]
        public async Task SimpleQueryNoParams()
        {
            var qp = new SqlServerQueryProvider(LocalDb.GetConnectionString("master"));
            var result = await qp.ExecuteAsync(new Query()
            {
                CommandType = CommandType.Text,
                Sql = "SELECT * FROM [sys].[tables]; SELECT * FROM [sys].[columns]"
            });

            Assert.IsTrue(result.DataSet.Tables.Count == 2);
            Assert.IsTrue(result.SchemaTables.Count == 2);
        }

        [TestMethod]
        public async Task SimpleQueryWithParams()
        {
            var qp = new SqlServerQueryProvider(LocalDb.GetConnectionString("master"));
            var result = await qp.ExecuteAsync(new Query()
            {
                CommandType = CommandType.Text,
                Sql = "SELECT * FROM [sys].[tables] WHERE [name] LIKE CONCAT('%', @name, '%')",
                Parameters = new Query.Parameter[]
                {
                    new Query.Parameter()
                    {
                        Name = "name",
                        Type = (int)SqlDbType.NVarChar,
                        Value = "de"
                    }
                }.ToList()
            });

            Assert.IsTrue(result.DataSet.Tables[0].Rows.Count == 1);
            Assert.IsTrue(result.DataSet.Tables[0].AsEnumerable().All(row => (row.Field<string>("name") ?? string.Empty).Contains("e")));
        }
    }
}