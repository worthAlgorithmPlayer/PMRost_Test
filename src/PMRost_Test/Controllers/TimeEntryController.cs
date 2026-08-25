using Microsoft.AspNetCore.Mvc;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.TimeEntries;
using PMRost_Test.Features.TimeEntries;

namespace PMRost_Test.Controllers;

[Route("api/time-entries")]
public sealed class TimeEntryController : ApiController
{
    /// <summary>
    /// Получение записей табеля
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ModelPagedResult<TimeEntryModel>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery] TimeEntryFilter filter,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllTimeEntriesQuery(filter), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Создание новой записи в табеле
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTimeEntryRequest request, CancellationToken cancellationToken)
    {
        var id = await Sender.Send(new CreateTimeEntryCommand(request.EmployeeId, request.ProjectId,
            request.TimesheetDate, request.Hours, request.Comment), cancellationToken);

        return CreatedAtRoute("", id);
    }

    /// <summary>
    /// Обновление существующей записи
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateTimeEntryRequest request, CancellationToken cancellationToken)
    {
        await Sender.Send(new UpdateTimeEntryCommand(id, request.Hours, request.Comment, request.Version), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаление записи из табеля
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await Sender.Send(new DeleteTimeEntryCommand(id), cancellationToken);
        return NoContent();
    }
}
