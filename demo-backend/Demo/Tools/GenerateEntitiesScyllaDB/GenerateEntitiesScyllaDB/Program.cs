// See https://aka.ms/new-console-template for more information
using GenerateEntitiesScyllaDB;
if (args.Length < 1)
{
    Console.WriteLine("⚠️  Usage: dotnet run -- <keyspace> [outputPath]");
    return;
}

string keySpace = args[0];
string outputPath = args.Length > 1 ? args[1] : "./Entities";
try
{
    EntityGenerator.GenerateEntities(keySpace, outputPath);
    Console.WriteLine($"✅ Entities generated for keyspace '{keySpace}' at {Path.GetFullPath(outputPath)}");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
}