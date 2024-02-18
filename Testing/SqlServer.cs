using Dapper;
using SqlServer.LocalDb;
using System.Data;
using System.Reflection;
using Zinger.Service;
using Zinger.Service.Models;
using Zinger.Service.QueryProviders;

namespace Testing;

[TestClass]
public class SqlServer
{
    private string GetContent(string resourceName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource not found: {resourceName}");
        return new StreamReader(stream).ReadToEnd();
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
        Assert.IsTrue(result.ResultClasses[0].Equals(GetContent("Testing.Resources.TableResult.txt")));
        Assert.IsTrue(result.ResultClasses[1].Equals(GetContent("Testing.Resources.ColumnResult.txt")));
    }

    [TestMethod]
    public async Task SimpleQueryWithParams()
    {
        var qp = new SqlServerQueryProvider(LocalDb.GetConnectionString("master"));
        var result = await qp.ExecuteAsync(new Query()
        {
            CommandType = CommandType.Text,
            Sql = "SELECT * FROM [sys].[tables] WHERE [name] LIKE CONCAT('%', @name, '%')",
            Parameters = [ new() { Name = "name", Type = DbType.String, Value = "de"} ]
        });

        Assert.IsTrue(result.DataSet.Tables[0].Rows.Count == 1);
        Assert.IsTrue(result.DataSet.Tables[0].AsEnumerable().All(row => (row.Field<string>("name") ?? string.Empty).Contains("e")));
	Assert.IsTrue(result.ResultClasses[0].Equals(
@"public class Result1
{
[MaxLength(128)]
[Required]
public string name { get; set; }
[Column(""object_id"")]
public int ObjectId { get; set; }
[Column(""principal_id"")]
public int? PrincipalId { get; set; }
[Column(""schema_id"")]
public int SchemaId { get; set; }
[Column(""parent_object_id"")]
public int ParentObjectId { get; set; }
[MaxLength(2)]
public string type { get; set; }
[Column(""type_desc"")]
[MaxLength(60)]
public string TypeDesc { get; set; }
[Column(""create_date"")]
public DateTime CreateDate { get; set; }
[Column(""modify_date"")]
public DateTime ModifyDate { get; set; }
[Column(""is_ms_shipped"")]
public bool IsMsShipped { get; set; }
[Column(""is_published"")]
public bool IsPublished { get; set; }
[Column(""is_schema_published"")]
public bool IsSchemaPublished { get; set; }
[Column(""lob_data_space_id"")]
public int LobDataSpaceId { get; set; }
[Column(""filestream_data_space_id"")]
public int? FilestreamDataSpaceId { get; set; }
[Column(""max_column_id_used"")]
public int MaxColumnIdUsed { get; set; }
[Column(""lock_on_bulk_load"")]
public bool LockOnBulkLoad { get; set; }
[Column(""uses_ansi_nulls"")]
public bool? UsesAnsiNulls { get; set; }
[Column(""is_replicated"")]
public bool? IsReplicated { get; set; }
[Column(""has_replication_filter"")]
public bool? HasReplicationFilter { get; set; }
[Column(""is_merge_published"")]
public bool? IsMergePublished { get; set; }
[Column(""is_sync_tran_subscribed"")]
public bool? IsSyncTranSubscribed { get; set; }
[Column(""has_unchecked_assembly_data"")]
public bool HasUncheckedAssemblyData { get; set; }
[Column(""text_in_row_limit"")]
public int? TextInRowLimit { get; set; }
[Column(""large_value_types_out_of_row"")]
public bool? LargeValueTypesOutOfRow { get; set; }
[Column(""is_tracked_by_cdc"")]
public bool? IsTrackedByCdc { get; set; }
[Column(""lock_escalation"")]
public byte? LockEscalation { get; set; }
[Column(""lock_escalation_desc"")]
[MaxLength(60)]
public string LockEscalationDesc { get; set; }
[Column(""is_filetable"")]
public bool? IsFiletable { get; set; }
[Column(""is_memory_optimized"")]
public bool? IsMemoryOptimized { get; set; }
public byte? durability { get; set; }
[Column(""durability_desc"")]
[MaxLength(60)]
public string DurabilityDesc { get; set; }
[Column(""temporal_type"")]
public byte? TemporalType { get; set; }
[Column(""temporal_type_desc"")]
[MaxLength(60)]
public string TemporalTypeDesc { get; set; }
[Column(""history_table_id"")]
public int? HistoryTableId { get; set; }
[Column(""is_remote_data_archive_enabled"")]
public bool? IsRemoteDataArchiveEnabled { get; set; }
[Column(""is_external"")]
public bool IsExternal { get; set; }
[Column(""history_retention_period"")]
public int? HistoryRetentionPeriod { get; set; }
[Column(""history_retention_period_unit"")]
public int? HistoryRetentionPeriodUnit { get; set; }
[Column(""history_retention_period_unit_desc"")]
[MaxLength(10)]
public string HistoryRetentionPeriodUnitDesc { get; set; }
[Column(""is_node"")]
public bool? IsNode { get; set; }
[Column(""is_edge"")]
public bool? IsEdge { get; set; }
}"));
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