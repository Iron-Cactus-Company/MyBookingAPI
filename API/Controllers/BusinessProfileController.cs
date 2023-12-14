using Application.BusinessProfileActions;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BusinessProfileController : BaseApiController{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BusinessProfile body)
    {
        var result = await Mediator.Send(new Create.Command{BusinessProfile = body});
        return HandleResponse(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMany()
    {
        var result = await Mediator.Send(new GetMany.Query());
        return HandleResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityById([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetOne.Query{ Id = id});
        return HandleResponse(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateById([FromBody] BusinessProfile body)
    {
        var result = await Mediator.Send(new Update.Command{ BusinessProfile = body });
        return HandleResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new Delete.Command{ Id = id});
        return HandleResponse(result);
    }
}