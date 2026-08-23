
using MongoDB.Driver;
using PMRost_Test.DAL.Mongo;

namespace PMRost_Test.DAL.Migrations;

public static class MongoSchemaMigrator
{
    public static async Task MigrateSchema(IMongoDatabase database, CancellationToken cancellationToken = default)
    {
        await MongoClassMapRegistrar.EnsureIndexesAsync(database, cancellationToken).ConfigureAwait(false);
    }
}
