
using PMRost_Test.Application.Contracts.Projects;
using PMRost_Test.Common.DataAccess;

namespace PMRost_Test.Application.Contracts.Repositories;

public interface IProjectReadRepository : IApplicationReadRepository
{
    public Task<ModelPagedResult<ProjectModel>> GetAllAsync(ProjectFilter filter, CancellationToken cancellationToken);
}
