using API.Contracts.BusinessProfile;
using Application.BusinessProfileActions;
using Application.Core.Error.Enums;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BusinessProfileController : BaseApiController{
        
    public BusinessProfileController()
    {
    }
    
    // [HttpGet]
    // public async Task<IActionResult> GetMany()
    // {
    //     var result = await Mediator.Send(new GetMany.Query());
    //     var serializedResult = _mapper.Map<IList<BusinessProfileResponseObject>>(result.Value);
    //         
    //     return Ok(serializedResult);
    // }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProfileById([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetOne.Query{ Id = id });
        if (result.Error.Type == ErrorType.NotFound)
            return NotFound(result.Error);

        //Only owner may read
        if (!IsProfileOwner(id))
            return Unauthorized();
        
        return HandleReadResponse<BusinessProfile, BusinessProfileResponseObject>(result);
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBusinessProfileDto createBusinessProfileDto)
    {
        var result = await Mediator.Send(new Create.Command
        {
            BusinessProfile = Mapper.Map<BusinessProfile>(createBusinessProfileDto)
        });
        
        return HandleCreateResponse(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateById([FromBody] UpdateBusinessProfileDto updateBusinessProfileDto)
    {
        //Only owner may update
        if (!IsProfileOwner(updateBusinessProfileDto.Id))
            return Unauthorized();
       
        var result = await Mediator.Send(new Update.Command
        {
            BusinessProfile = Mapper.Map<BusinessProfile>(updateBusinessProfileDto)
        });
            
        return HandleUpdateResponse(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        //Only owner may delete
        if (!IsProfileOwner(id))
            return Unauthorized();
        
        var result = await Mediator.Send(new Delete.Command{ Id = id});
        
        return HandleDeleteResponse(result);
    }

    private bool IsProfileOwner(Guid requestId)
    {
        return requestId.ToString() == GetLoggedUserId();
    }
    private bool IsProfileOwner(string requestId)
    {
        return requestId == GetLoggedUserId();
    }
}