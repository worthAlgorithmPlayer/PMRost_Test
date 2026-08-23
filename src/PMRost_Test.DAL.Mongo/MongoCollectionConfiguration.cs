
using MongoDB.Driver;

namespace PMRost_Test.DAL.Mongo;

public interface IMongoCollectionConfiguration
{
    /// <summary>Регистрирует BsonClassMap для сущности. Должен быть идемпотентным.</summary>
    void ConfigureClassMap();

    Task EnsureIndexesAsync(IMongoDatabase database, CancellationToken cancellationToken = default);
}


public abstract class MongoCollectionConfiguration<T> : IMongoCollectionConfiguration
{
    protected abstract string CollectionName { get; }

    public abstract void ConfigureClassMap();

    /// <summary>
    /// Аналог builder.HasIndex() EF конфигурации
    /// </summary>
    protected virtual IEnumerable<CreateIndexModel<T>> Indexes() => Enumerable.Empty<CreateIndexModel<T>>();

    public async Task EnsureIndexesAsync(IMongoDatabase database, CancellationToken cancellationToken = default)
    {
        var collection = database.GetCollection<T>(CollectionName);
        var models = Indexes().ToList();

        if (models.Count > 0)
        {
            await collection.Indexes
                .CreateManyAsync(models, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
