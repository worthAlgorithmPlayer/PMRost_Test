
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace PMRost_Test.DAL.Mongo;

public static class ServiceCollectionExtensions
{
    private const string ConnectionName = "PmRostTestDatabaseMongo";

    public static IServiceCollection AddPmRostTestContextMongo(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionName)
            ?? throw new InvalidOperationException(
                $"Не удалось получить строку подключения: {ConnectionName}");

        var databaseName = configuration.GetSection(MongoDbOptions.SectionName)["DatabaseName"]
            ?? throw new InvalidOperationException(
                $"Не удалось получить имя базы данных из секции: {MongoDbOptions.SectionName}");

        var mongoDbOptions = new MongoDbOptions
        {
            ConnectionString = connectionString,
            DatabaseName = databaseName
        };
        services.AddSingleton(mongoDbOptions);

        services.AddSingleton<IMongoClient>(_ =>
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);

            return new MongoClient(settings);
        });

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        services.AddScoped<PMRostTestContextMongo>();

        MongoClassMapRegistrar.RegisterAll();

        return services;
    }
}
