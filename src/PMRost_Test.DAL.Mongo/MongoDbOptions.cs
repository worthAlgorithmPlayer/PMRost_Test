
namespace PMRost_Test.DAL.Mongo;

public sealed class MongoDbOptions
{
    public const string SectionName = "MongoDbOptions";

    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}
