
using Mediator;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Employees;

namespace PMRost_Test.Features.Employees;

public sealed class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, ModelPagedResult<EmployeeModel>>
{
    public ValueTask<ModelPagedResult<EmployeeModel>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public record GetAllEmployeesQuery(EmployeeFilter Filter) : IQuery<ModelPagedResult<EmployeeModel>>;