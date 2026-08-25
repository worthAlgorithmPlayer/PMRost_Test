using MongoDB.Driver;
using PMRost_Test;
using PMRost_Test.DAL.Migrations;
using PMRost_Test.Services.MockData;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var configuration = host.Services.GetRequiredService<IConfiguration>();

        //await MigrateSchema(configuration);
        using (var scope = host.Services.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            await MongoSchemaMigrator.MigrateSchema(database);
        }

        if (args.Contains("--seed"))
        {
            using (var scope = host.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<MockDataSeeder>();

                Console.WriteLine("--> Executing MockDataSeeder...");
                await seeder.SeedAsync();
                Console.WriteLine("--> MockDataSeeder finished successfully.");
            }

            return;
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}