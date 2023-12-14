using System.Security.Claims;
using API.Enum;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase{
    private IMediator _mediator;
    //Use the _mediator field, but if it is not defined use the one defined in the Program.cs
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected string GetLoggedUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    
    protected ProfileType GetLoggedUserProfileType()
    {
        string profileTypeString = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        int type;
        int.TryParse(profileTypeString, out type);

        return (ProfileType)type;
    }

    protected IActionResult HandleResponse<T>(Result<T> result)
    {
        if (result == null)
            return NotFound();
        if (result.IsSuccess && result.Value != null)
            return Ok(result);
        if (result.IsSuccess && result.Value == null)
            return NotFound();

        return BadRequest();
    }
}