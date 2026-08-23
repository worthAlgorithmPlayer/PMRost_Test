
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace PMRost_Test.DAL.Migrations;

public interface IMongoDataMigration
{
    string Version { get; }

    Task UpAsync(IMongoDatabase database, CancellationToken cancellationToken);
}

internal sealed class MongoMigrationHistoryRecord
{
    [BsonId]
    public string Version { get; set; } = default!;

    public DateTime AppliedAtUtc { get; set; }
}

public static class MongoDataMigrationManager
{
    private const string HistoryCollectionName = "__mongoDataMigrationsHistory";

    public static async Task MigrateData(IMongoDatabase database, CancellationToken cancellationToken = default)
    {
        var history = database.GetCollection<MongoMigrationHistoryRecord>(HistoryCollectionName);

        var appliedVersions = (await history
                .Find(FilterDefinition<MongoMigrationHistoryRecord>.Empty)
                .Project(x => x.Version)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false))
            .ToHashSet(StringComparer.Ordinal);

        var pendingMigrations = DiscoverMigrations()
            .Where(m => !appliedVersions.Contains(m.Version))
            .OrderBy(m => m.Version, StringComparer.Ordinal);

        foreach (var migration in pendingMigrations)
        {
            await migration.UpAsync(database, cancellationToken).ConfigureAwait(false);

            await history.InsertOneAsync(
                new MongoMigrationHistoryRecord { Version = migration.Version, AppliedAtUtc = DateTime.UtcNow },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }

    private static IEnumerable<IMongoDataMigration> DiscoverMigrations()
    {
        return typeof(MongoDataMigrationManager).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && typeof(IMongoDataMigration).IsAssignableFrom(t))
            .Select(t => (IMongoDataMigration)(Activator.CreateInstance(t)
                ?? throw new InvalidOperationException($"Не удалось создать миграцию {t.Name}")));
    }
}
