using System.Reflection;
using Zinger.Service.Models;

namespace Testing;

[TestClass]
public class Queries
{
	[TestMethod]
	[DataRow("Testing.Resources.SampleQuery.sql")]
	public void ParseQueries(string resourceName)
	{
		var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource not found: {resourceName}");
		var query = Query.FromStream(stream);
	}
}
