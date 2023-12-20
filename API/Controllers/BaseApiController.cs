using System.Security.Claims;
using API.Enum;
using Application.Core;
using Application.Core.Error;
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

    protected IActionResult HandleCreateResponse<TResult, TResponse>(Result<TResult> result)
    {
        if (result.Value != null)
        {
            var serializedResult = Mapper.Map<TResponse>(result.Value);
            return Ok(new ObjectResponseToClient<TResponse>{ Data = serializedResult });
        }
        
        if (result.IsSuccess)
            return Created();

        var errorResp = new ObjectResponseToClient<TResponse> { Error = result.Error };
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(errorResp);
        if (result.Error.Type == ErrorType.NotUnique)
            return Conflict(errorResp);

        return BadRequest(errorResp);
    }
    
    protected IActionResult HandleReadOneResponse<TResult, TResponse>(Result<TResult> result)
    {
        if (result.IsSuccess)
        {
            var serializedResult = Mapper.Map<TResponse>(result.Value);
            return Ok(new ObjectResponseToClient<TResponse>{ Data = serializedResult });
        }
        
        var errorResp = new ObjectResponseToClient<TResponse> { Error = result.Error };
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(errorResp);

        return BadRequest(errorResp);
    }
    
    protected IActionResult HandleReadManyResponse<TResult, TResponse>(Result<TResult> result)
    {
        if (result.IsSuccess)
        {
            var serializedResult = Mapper.Map<TResponse>(result.Value);
            return Ok(new ReadManyResponseToClient<TResponse>{ Data = serializedResult });
        }
        
        var errorResp = new ReadManyResponseToClient<TResponse> { Error = result.Error };
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(errorResp);

        return BadRequest(errorResp);
    }
    
    protected IActionResult HandleUpdateResponse<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return NoContent();
        
        var errorResp = new ObjectResponseToClient<T> { Error = result.Error };
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(errorResp);
        if (result.Error.Type == ErrorType.NotUnique)
            return Conflict(errorResp);

        return BadRequest(errorResp);
    }
    
    protected IActionResult HandleDeleteResponse<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return NoContent();
        
        var errorResp = new ObjectResponseToClient<T> { Error = result.Error };
        
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(errorResp);

        return BadRequest(errorResp);
    }
    
    protected class ObjectResponseToClient<T>
    {
        public T Data { get; set; } = default;
        public ApplicationRequestError Error { get; set; } = default;
    } 
    
    protected class ReadManyResponseToClient<T>
    {
        public T Data { get; set; } = default;
        public ApplicationRequestError Error { get; set; } = default;
    }
}