using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace PMRost_Test.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private ISender? _sender;

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
