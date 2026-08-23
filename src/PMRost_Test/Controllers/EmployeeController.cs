using Microsoft.AspNetCore.Mvc;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Employees;
using PMRost_Test.Features.Employees;

namespace PMRost_Test.Controllers;

[Route("api/v1/employees")]
public sealed class EmployeeController : ApiController
{
    /// <summary>
    /// Получение пользователей
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ModelPagedResult<EmployeeModel>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetConversationMessagesAsync([FromQuery] EmployeeFilter filter,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllEmployeesQuery(filter), cancellationToken);

        return Ok(result);
    }
}
