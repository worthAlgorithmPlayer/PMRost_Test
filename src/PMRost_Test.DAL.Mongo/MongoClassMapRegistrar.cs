
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PMRost_Test.Domain.Primitives.EntityTemplates;
namespace PMRost_Test.DAL.Mongo;

public static class MongoClassMapRegistrar
{
    private static readonly object RegisterLock = new();
    private static bool _registered;
    public static void RegisterAll()
    {
        lock (RegisterLock)
        {
            if (_registered)
            {
                return;
            }

            RegisterBaseClassMaps();

            foreach (var configuration in DiscoverConfigurations())
            {
                configuration.ConfigureClassMap();
            }

            _registered = true;
        }
    }

    public static async Task EnsureIndexesAsync(IMongoDatabase database, CancellationToken cancellationToken = default)
    {
        foreach (var configuration in DiscoverConfigurations())
        {
            await configuration.EnsureIndexesAsync(database, cancellationToken).ConfigureAwait(false);
        }
    }

    private static IEnumerable<IMongoCollectionConfiguration> DiscoverConfigurations()
    {
        return typeof(MongoClassMapRegistrar).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && typeof(IMongoCollectionConfiguration).IsAssignableFrom(t))
            .Select(t => (IMongoCollectionConfiguration)(Activator.CreateInstance(t)
                ?? throw new InvalidOperationException($"Не удалось создать конфигурацию {t.Name}")));
    }

    private static void RegisterBaseClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(DomainEntity)))
        {
            BsonClassMap.RegisterClassMap<DomainEntity>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdProperty(x => x.Id)
                  .SetSerializer(new GuidSerializer(BsonType.String));
            });
        }
    }
}