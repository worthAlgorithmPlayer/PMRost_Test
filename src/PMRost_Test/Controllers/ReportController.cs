using Microsoft.AspNetCore.Mvc;
using PMRost_Test.Application.Contracts.Reports;
using PMRost_Test.Features.Reports;

namespace PMRost_Test.Controllers;

[Route("api/reports/projects")]
public class ReportController : ApiController
{
    /// <summary>
    /// Отчёт по проектам за месяц
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(MonthlyProjectReportResult),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReportAsync([FromQuery] GetProjectReportRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetMonthlyProjectReportQuery(request.Year, request.Month), cancellationToken);

        return Ok(result);
    }

}
