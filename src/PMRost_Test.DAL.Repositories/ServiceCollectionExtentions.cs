
using Microsoft.Extensions.DependencyInjection;
using PMRost_Test.Common;
using PMRost_Test.Common.DataAccess;

namespace PMRost_Test.DAL.Repositories;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
      services.AddApplicationRepositories();

    private static IServiceCollection AddApplicationRepositories(this IServiceCollection services) =>
        services.AddAllTypesAssignableMarkerInterfaceTo<IApplicationReadRepository>(
            RepositoryAssemblyReference.Assembly, ServiceLifetime.Transient);
}
