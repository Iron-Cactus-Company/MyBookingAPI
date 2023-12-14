using API.Contracts.BusinessProfile;
using Application.BusinessProfileActions;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BusinessProfileController : BaseApiController{
    private readonly IMapper _mapper;
        
    public BusinessProfileController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMany()
    {
        var result = await Mediator.Send(new GetMany.Query());
        var serializedResult = _mapper.Map<IList<BusinessProfileResponseObject>>(result.Value);
            
        return Ok(serializedResult);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetActivityById([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetOne.Query{ Id = id});
        if (result.Value == null)
        {
            return NotFound();
        }
        var serializedResult = _mapper.Map<BusinessProfileResponseObject>(result.Value);
        return Ok(serializedResult);
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBusinessProfileDto createBusinessProfileDto)
    {
        var result = await Mediator.Send(new Create.Command
        {
            BusinessProfile = _mapper.Map<BusinessProfile>(createBusinessProfileDto)
        });
        if (!result.IsSuccess)
        {
            return Conflict();
        }
        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateById([FromBody] UpdateBusinessProfileDto updateBusinessProfileDto)
    {
       
        var result = await Mediator.Send(new Update.Command
        {
            BusinessProfile = _mapper.Map<BusinessProfile>(updateBusinessProfileDto)
        });
            
        if (!result.IsSuccess)
        {
            return Conflict();
        }
        return Ok(result);
        
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        //todo fix internal error 500 when notfound
        var result = await Mediator.Send(new Delete.Command{ Id = id});
        if (!result.IsSuccess)
        {
            return Conflict();
        }
        return NoContent();
    }
}