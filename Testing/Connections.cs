﻿using Zinger.Service;
using Zinger.Service.Models;

namespace Testing;

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
				Type = DatabaseType.SqlLite
			},
			new Connection()
			{
				Name = "that",
				ConnectionString = "connection string bravo",
				Type = DatabaseType.OleDb
			},
			new Connection()
			{
				Name = "other",
				ConnectionString = "connection string charlie",
				Type = DatabaseType.MySql
			}
		};

		var store = new LocalConnectionStore();
		foreach (var cn in  connections) await store.SaveAsync(cn);
		
		var result = (await store.GetAllAsync().ToArrayAsync()).OrderBy(row => row.Name);

		// not working because I have some other live connections on my local machine
		Assert.IsTrue(result.SequenceEqual(connections.OrderBy(row => row.Name)));
	}
}
