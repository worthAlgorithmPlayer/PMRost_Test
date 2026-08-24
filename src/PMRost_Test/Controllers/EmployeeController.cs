using Microsoft.AspNetCore.Mvc;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Employees;
using PMRost_Test.Features.Employees;

namespace PMRost_Test.Controllers;

[Route("api/employees")]
public sealed class EmployeeController : ApiController
{
    /// <summary>
    /// Получение пользователей
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ModelPagedResult<EmployeeModel>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmployeesAsync([FromQuery] EmployeeFilter filter,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllEmployeesQuery(filter), cancellationToken);

        return Ok(result);
    }
}
