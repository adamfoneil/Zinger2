using Zinger2.Service;
using Zinger2.Service.Models;

namespace Testing
{
    [TestClass]
    public class Connections
    {
        [TestMethod]
        public async Task TestConnectionStore()
        {
            var connections = new[]
            {
                new Connection()
                {
                    Name = "this",
                    ConnectionString = "connection string alpha",
                    Type = ConnectionType.SqlLite
                },
                new Connection()
                {
                    Name = "that",
                    ConnectionString = "connection string bravo",
                    Type = ConnectionType.OleDb
                },
                new Connection()
                {
                    Name = "other",
                    ConnectionString = "connection string charlie",
                    Type = ConnectionType.MySql
                }
            };

            var store = new ConnectionStore();
            await store.SaveConnectionsAsync(connections);

            var result = await store.GetConnectionsAsync();
            Assert.IsTrue(result.SequenceEqual(connections));
        }
    }
}
