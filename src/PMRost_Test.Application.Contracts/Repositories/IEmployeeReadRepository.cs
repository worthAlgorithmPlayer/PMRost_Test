
using PMRost_Test.Application.Contracts.Employees;
using PMRost_Test.Common.DataAccess;

namespace PMRost_Test.Application.Contracts.Repositories;

public interface IEmployeeReadRepository : IApplicationReadRepository
{
    public Task<ModelPagedResult<EmployeeModel>> GetAllAsync(EmployeeFilter filter, CancellationToken cancellationToken);
}
