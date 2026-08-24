
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Projects;
using PMRost_Test.Application.Contracts.Repositories;
using PMRost_Test.DAL.Mongo;
using PMRost_Test.Domain;

namespace PMRost_Test.DAL.Repositories.Application;

internal sealed class ProjectReadRepository : IProjectReadRepository
{
    private readonly PMRostTestContextMongo _dbContext;

    public ProjectReadRepository(PMRostTestContextMongo dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ModelPagedResult<ProjectModel>> GetAllAsync(ProjectFilter filter, CancellationToken cancellationToken)
    {
        var totalCount = await _dbContext.Projects
            .CountDocumentsAsync(FilterDefinition<Project>.Empty, cancellationToken: cancellationToken);

        var items = await _dbContext.Projects
            .AsQueryable()
            .OrderBy(x => x.Number)
            .Skip(filter.Skip)
            .Take(filter.Limit)
            .Select(x => new ProjectModel
            {
                Id = x.Id,
                Number = x.Number,
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            })
            .ToListAsync(cancellationToken);

        var result = new ModelPagedResult<ProjectModel>
        {
            TotalCount = totalCount,
            Items = items
        };

        return result;
    }
}
