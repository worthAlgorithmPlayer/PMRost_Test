
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Employees;
using PMRost_Test.Application.Contracts.Repositories;
using PMRost_Test.DAL.Mongo;
using PMRost_Test.Domain;

namespace PMRost_Test.DAL.Repositories.Application;

public class EmployeeReadRepository : IEmployeeReadRepository
{
    private readonly PMRostTestContextMongo _dbContext;

    public EmployeeReadRepository(PMRostTestContextMongo dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ModelPagedResult<EmployeeModel>> GetAllAsync(EmployeeFilter filter, CancellationToken cancellationToken)
    {
        var totalCount = await _dbContext.Employees
            .CountDocumentsAsync(FilterDefinition<Employee>.Empty, cancellationToken: cancellationToken);

        var items = await _dbContext.Employees
            .AsQueryable()
            .OrderBy(x => x.FullName)
            .Skip(filter.Skip)
            .Take(filter.Limit)
            .Select(x => new EmployeeModel
            {
                Id = x.Id,
                Name = x.FullName,
                Department = x.Department
            })
            .ToListAsync(cancellationToken);

        var result = new ModelPagedResult<EmployeeModel>
        {
            TotalCount = totalCount,
            Items = items
        };

        return result;
    }
}
