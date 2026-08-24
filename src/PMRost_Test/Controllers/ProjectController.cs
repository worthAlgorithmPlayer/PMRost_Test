using Microsoft.AspNetCore.Mvc;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Projects;
using PMRost_Test.Features.Projects;

namespace PMRost_Test.Controllers;

[Route("api/projects")]
public sealed class ProjectController : ApiController
{
    /// <summary>
    /// Получение проектов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ModelPagedResult<ProjectModel>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProjectsAsync([FromQuery] ProjectFilter filter,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllProjectsQuery(filter), cancellationToken);

        return Ok(result);
    }
}
