
namespace PMRost_Test.Application.Contracts.Employees;

public sealed class EmployeeFilter
{
    public int Skip { get; set; } = 0;
    public int Limit { get; set; } = 10;
}
