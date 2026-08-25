using PMRost_Test.DAL.Mongo;
using PMRost_Test.DAL.Repositories;
using PMRost_Test.Domain.TimeEntries.Services;
using PMRost_Test.Features;
using PMRost_Test.Middlewares;

namespace PMRost_Test;

internal sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddControllers();

        services.AddSwaggerGen();

        services.AddScoped<ITimeEntryService, TimeEntryService>();

        services.AddPmRostTestContextMongo(_configuration);

        services.AddRepositories();

        services.AddEndpointsApiExplorer();

        services.AddScoped<MockDataSeeder>();

        services.AddFeaturesLayer(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseRouting();

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PmRostTestWork API V1");
            c.RoutePrefix = "swagger";
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
