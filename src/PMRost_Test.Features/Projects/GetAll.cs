
using Mediator;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Projects;
using PMRost_Test.Application.Contracts.Repositories;

namespace PMRost_Test.Features.Projects;

public sealed class GetAllProjectsQueryHandler : IQueryHandler<GetAllProjectsQuery, ModelPagedResult<ProjectModel>>
{
    private readonly IProjectReadRepository _projectReadRepository;

    public GetAllProjectsQueryHandler(IProjectReadRepository projectReadRepository)
    {
        _projectReadRepository = projectReadRepository;
    }

    public async ValueTask<ModelPagedResult<ProjectModel>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken)
    {
        return await _projectReadRepository.GetAllAsync(query.Filter, cancellationToken);
    }
}

public record GetAllProjectsQuery(ProjectFilter Filter) : IQuery<ModelPagedResult<ProjectModel>>;