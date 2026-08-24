
using Mediator;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Employees;
using PMRost_Test.Application.Contracts.Repositories;

namespace PMRost_Test.Features.Employees;

public sealed class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, ModelPagedResult<EmployeeModel>>
{
    private readonly IEmployeeReadRepository _employeeReadRepository;

    public GetAllEmployeesQueryHandler(IEmployeeReadRepository employeeReadRepository)
    {
        _employeeReadRepository = employeeReadRepository;
    }

    public async ValueTask<ModelPagedResult<EmployeeModel>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        return await _employeeReadRepository.GetAllAsync(query.Filter, cancellationToken);
    }
}

public record GetAllEmployeesQuery(EmployeeFilter Filter) : IQuery<ModelPagedResult<EmployeeModel>>;