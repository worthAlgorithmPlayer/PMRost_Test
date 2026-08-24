
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PMRost_Test.Features;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFeaturesLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Transient);

        services.AddValidatorsFromAssemblyContaining<FeatureAssemblyReference>();

        return services;
    }
}
