using System.Runtime.InteropServices.JavaScript;
using API.Contracts.Auth;
using API.Contracts.BusinessProfile;
using API.Enum;
using API.Service;
using Application.BusinessProfileActions;
using Application.Core.Security;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : BaseApiController
{
    public AuthController(TokenService tokenService, IMapper mapper, PasswordManager passwordManager)
    {
        _tokenService = tokenService;
        _mapper = mapper;
        _passwordManager = passwordManager;
    }
    
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly PasswordManager _passwordManager;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseObject>> Login([FromBody] LoginDto loginDto)
    {
        var businessProfile = await Mediator.Send(new FindOneByEmail.Query{ Email = loginDto.Email });

        if (businessProfile.Value == null || !_passwordManager.VerifyPassword(businessProfile.Value.Password, loginDto.Password))
            return Unauthorized();
        
        var token = _tokenService.GenerateToken(businessProfile.Value.Id.ToString(), ProfileType.Business);

        var loginUserResponseObject = _mapper.Map<LoginUserResponseObject>(businessProfile.Value);
        loginUserResponseObject.AccessToken = token;
        return loginUserResponseObject;
    }
    
    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        var businessProfile = await Mediator.Send(new GetOne.Query { Id = new Guid(GetLoggedUserId()) });
        if(!businessProfile.IsSuccess)
            return HandleReadOneResponse<BusinessProfile, BusinessProfileResponseObject>(businessProfile);
        
        var company = await Mediator.Send(
            new Application.CompanyActions.FindOneByBusinessProfileId.Query
            {
                BusinessProfileId = new Guid(GetLoggedUserId())
            });
        
        var resp = new UserResponseObject { Id = businessProfile.Value.Id.ToString(), Email = businessProfile.Value.Email, Company = company.Value};
        var serializedResult = Mapper.Map<UserResponseObject>(resp);
        return Ok(new ObjectResponseToClient<UserResponseObject>{ Data = serializedResult });
    }
}