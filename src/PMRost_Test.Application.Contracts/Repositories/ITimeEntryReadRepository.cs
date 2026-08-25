

using PMRost_Test.Application.Contracts.TimeEntries;
using PMRost_Test.Common.DataAccess;

namespace PMRost_Test.Application.Contracts.Repositories;

public interface ITimeEntryReadRepository : IApplicationReadRepository
{
    public Task<ModelPagedResult<TimeEntryModel>> GetAllAsync(TimeEntryFilter filter, CancellationToken cancellationToken);
}
