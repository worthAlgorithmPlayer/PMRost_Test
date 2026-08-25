
using Mediator;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Repositories;
using PMRost_Test.Application.Contracts.TimeEntries;

namespace PMRost_Test.Features.TimeEntries;

public sealed class GetAllTimeEntriesQueryHandler : IQueryHandler<GetAllTimeEntriesQuery, ModelPagedResult<TimeEntryModel>>
{
    private readonly ITimeEntryReadRepository _timeEntryReadRepository;

    public GetAllTimeEntriesQueryHandler(ITimeEntryReadRepository timeEntryReadRepository)
    {
        _timeEntryReadRepository = timeEntryReadRepository;
    }

    public async ValueTask<ModelPagedResult<TimeEntryModel>> Handle(GetAllTimeEntriesQuery query, CancellationToken cancellationToken)
    {
        return await _timeEntryReadRepository.GetAllAsync(query.Filter, cancellationToken);
    }
}

public record GetAllTimeEntriesQuery(TimeEntryFilter Filter) : IQuery<ModelPagedResult<TimeEntryModel>>;