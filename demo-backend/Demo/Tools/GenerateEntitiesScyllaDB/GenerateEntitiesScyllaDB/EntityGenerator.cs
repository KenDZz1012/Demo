using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;

namespace GenerateEntitiesScyllaDB
{
    public class EntityGenerator
    {
        public static void GenerateEntities(string keyspace, string outputPath)
        {
            var cluster = Cluster.Builder()
                .AddContactPoint("103.82.25.49")
                .WithPort(9042)
                .Build();

            var session = cluster.Connect("system_schema");

            var tables = session.Execute($"SELECT table_name FROM tables WHERE keyspace_name = '{keyspace}'");

            foreach (var table in tables)
            {
                string tableName = table.GetValue<string>("table_name");
                Console.WriteLine(tableName);
                var columns = session.Execute($@"
                SELECT column_name, type 
                FROM columns 
                WHERE keyspace_name = '{keyspace}' 
                AND table_name = '{tableName}'");

                var sb = new StringBuilder();
                sb.AppendLine("using Cassandra.Mapping.Attributes;");
                sb.AppendLine();
                sb.AppendLine($"[Table(\"{tableName}\", Keyspace = \"{keyspace}\")]");
                sb.AppendLine($"public class {ToPascalCase(tableName)}");
                sb.AppendLine("{");

                foreach (var col in columns)
                {
                    string colName = col.GetValue<string>("column_name");
                    string colType = MapCassandraTypeToCSharp(col.GetValue<string>("type"));
                    sb.AppendLine($"    [Column(\"{colName}\")]");
                    sb.AppendLine($"    public {colType} {ToPascalCase(colName)} {{ get; set; }}");
                    sb.AppendLine();
                }

                sb.AppendLine("}");
                Directory.CreateDirectory(outputPath); // đảm bảo thư mục tồn tại

                File.WriteAllText(
                    Path.Combine(outputPath, $"{ToPascalCase(tableName)}.cs"),
                    sb.ToString()
                );

                Console.WriteLine(Path.Combine(outputPath, $"{ToPascalCase(tableName)}.cs"));
            }
        }

        static string ToPascalCase(string name) =>
            string.Join("", name.Split('_').Select(w => char.ToUpper(w[0]) + w.Substring(1)));

        static string MapCassandraTypeToCSharp(string type) =>
            type switch
            {
                "text" => "string",
                "varchar" => "string",
                "uuid" => "Guid",
                "int" => "int",
                "bigint" => "long",
                "boolean" => "bool",
                "timestamp" => "DateTimeOffset",
                "double" => "double",
                _ => "string"
            };
    }
}
