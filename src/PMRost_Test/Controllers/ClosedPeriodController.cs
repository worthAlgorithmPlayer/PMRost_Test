using Microsoft.AspNetCore.Mvc;
using PMRost_Test.Application.Contracts.ClosedPeriods;
using PMRost_Test.Features.ClosedPeriods;

namespace PMRost_Test.Controllers;

[Route("api/periods")]
public class ClosedPeriodController : ApiController
{
    /// <summary>
    /// Закрыть месяц
    /// </summary>
    [HttpPost("close")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateClosedPeriodAsync([FromBody] ClosedPeriodCreateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new AddClosedPeriodCommand(request.Year, request.Month), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Открыть месяц
    /// </summary>
    [HttpPost("open")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClosedPeriodAsync([FromBody] ClosedPeriodDeleteRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new DeleteClosedPeriodCommand(request.Year, request.Month), cancellationToken);

        return Ok(result);
    }
}
