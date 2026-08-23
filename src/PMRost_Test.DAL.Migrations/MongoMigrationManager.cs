
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PMRost_Test.DAL.Mongo;

namespace PMRost_Test.DAL.Migrations;

public static class MongoMigrationManager
{
    private const string ConnectionName = "PMRostTestDatabaseMongo";

    public static async Task MigrateSchema(IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        var database = ResolveDatabase(configuration);
        await MongoSchemaMigrator.MigrateSchema(database, cancellationToken).ConfigureAwait(false);
    }

    public static async Task MigrateData(IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        var database = ResolveDatabase(configuration);
        await MongoDataMigrationManager.MigrateData(database, cancellationToken).ConfigureAwait(false);
    }

    private static IMongoDatabase ResolveDatabase(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionName)
            ?? throw new InvalidOperationException(
                $"Не удалось получить строку подключения: {ConnectionName}");

        var databaseName = configuration.GetSection(MongoDbOptions.SectionName)["DatabaseName"]
            ?? throw new InvalidOperationException(
                $"Не удалось получить имя базы данных из секции: {MongoDbOptions.SectionName}");

        var client = new MongoClient(connectionString);
        return client.GetDatabase(databaseName);
    }
}
