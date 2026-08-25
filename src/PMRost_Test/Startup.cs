using PMRost_Test.DAL.Mongo;
using PMRost_Test.DAL.Repositories;
using PMRost_Test.Domain.TimeEntries.Services;
using PMRost_Test.Features;
using PMRost_Test.Middlewares;
using PMRost_Test.Services.MockData;

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
        services.AddHttpClient();

        services.AddScoped<MockDataSeeder>();

        services.AddFeaturesLayer(_configuration);

        ConfigureCors(services);
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options => options.AddDefaultPolicy(policy =>
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors();
        app.UseRouting();

        app.UseMiddleware<ExceptionMiddleware>();

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
