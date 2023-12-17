using System.Security.Claims;
using API.Contracts.BusinessProfile;
using API.Enum;
using Application.Core;
using Application.Core.Error.Enums;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase{
    private IMediator _mediator;
    //Use the _mediator field, but if it is not defined use the one defined in the Program.cs
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    
    protected IMapper Mapper => HttpContext.RequestServices.GetService<IMapper>();

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

    protected IActionResult HandleCreateResponse<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Created();
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(result.Error);
        if (result.Error.Type == ErrorType.NotUnique)
            return Conflict(result.Error);

        return BadRequest();
    }
    
    protected IActionResult HandleReadResponse<TResult, TResponse>(Result<TResult> result)
    {
        if (result.IsSuccess)
        {
            var serializedResult = Mapper.Map<TResponse>(result.Value);
            return Ok(serializedResult);
        }
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(result.Error);

        return BadRequest();
    }
    
    protected IActionResult HandleUpdateResponse<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return NoContent();
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(result.Error);
        if (result.Error.Type == ErrorType.NotUnique)
            return Conflict(result.Error);

        return BadRequest();
    }
    
    protected IActionResult HandleDeleteResponse<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return NoContent();
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(result.Error);

        return BadRequest();
    }
}