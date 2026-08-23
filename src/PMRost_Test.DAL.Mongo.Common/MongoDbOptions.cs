
namespace PMRost_Test.DAL.Mongo;

public sealed class MongoDbOptions
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}
