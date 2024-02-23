using System.Reflection;
using System.Text.Json;
using Zinger.Service.Models;

namespace Testing;

[TestClass]
public class Queries
{
	[TestMethod]
	[DataRow("Testing.Resources.SampleQuery.sql")]
	public void ParseQueries(string resourceName)
	{
		var input = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource not found: {resourceName}");
		var queryActual = Query.FromStream(input);

		var outputResource = resourceName + ".json";
		var output = Assembly.GetExecutingAssembly().GetManifestResourceStream(outputResource) ?? throw new Exception($"Output not found: {outputResource}");
		var outputJson = new StreamReader(output).ReadToEnd();
		var queryExpected = JsonSerializer.Deserialize<Query>(outputJson) ?? throw new Exception("Query not deserialized");

		Assert.AreEqual(queryActual.Sql, queryExpected.Sql);

	}
}
