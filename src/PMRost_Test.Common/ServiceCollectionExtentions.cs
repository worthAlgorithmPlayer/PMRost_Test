
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PMRost_Test.Common;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddAllTypesAssignableMarkerInterfaceTo<TMarkerInterface>(
        this IServiceCollection services, Assembly assemblyWithImplementations,
        ServiceLifetime lifetime)
    {
        var types = assemblyWithImplementations.GetTypes()
            .Where(type => typeof(TMarkerInterface).IsAssignableFrom(type) && !type.IsAbstract);

        foreach (var type in types) services.AddAsImplementedInterfaces(type, lifetime);

        return services;
    }

    private static IServiceCollection AddAsImplementedInterfaces(this IServiceCollection services, Type type,
      ServiceLifetime lifetime)
    {
        var interfaces = type.GetInterfaces();
        var assignableInterfaces = interfaces.Where(i => i.IsAssignableFrom(type));
        foreach (var interfaceType in assignableInterfaces)
            services.Add(new ServiceDescriptor(interfaceType, type, lifetime));

        return services;
    }
}
