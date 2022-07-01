using Microsoft.CSharp;
using System.CodeDom;
using System.Data;

namespace Zinger2.Service.Internal
{
    internal static class CSharpClassBuilder
    {
        internal static string FromSchemaTable(string className, DataTable schemaTable)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<ColumnInfo> CSharpPropertiesFromSchemaTable(DataTable schemaTable)
        {
            // find duplicate column names so we can append an incremental digit to the end of the name
            var dupColumns = schemaTable.AsEnumerable()
                .GroupBy(row => row.Field<string>("ColumnName") ?? string.Empty)
                .Where(grp => grp.Count() > 1)
                .Select(grp => grp.Key)
                .ToDictionary(name => name, name => 0);

            List<ColumnInfo> results = new List<ColumnInfo>();

            var provider = new CSharpCodeProvider();
            
            foreach (DataRow row in schemaTable.Rows)
            {
                string columnName = row.Field<string>("ColumnName") ?? string.Empty;
                if (dupColumns.ContainsKey(columnName)) dupColumns[columnName]++;
                ColumnInfo columnInfo = new ColumnInfo()
                {
                    Name = columnName,
                    CSharpType = CSharpTypeName(provider, row.Field<Type>("DataType")),
                    IsNullable = row.Field<bool>("AllowDBNull"),
                    Index = (dupColumns.ContainsKey(columnName)) ? dupColumns[columnName] : 0
                };

                if (columnInfo.IsNullable && !columnInfo.CSharpType.ToLower().Equals("string")) columnInfo.CSharpType += "?";
                if (columnInfo.CSharpType.ToLower().Equals("string") && row.Field<int>("ColumnSize") < int.MaxValue) columnInfo.Size = row.Field<int>("ColumnSize");

                results.Add(columnInfo);
            }

            return results;
        }

        private static string CSharpTypeName(CSharpCodeProvider provider, Type type)
        {
            var typeRef = new CodeTypeReference(type);
            return provider.GetTypeOutput(typeRef).Replace("System.", string.Empty);
        }
    }
}
