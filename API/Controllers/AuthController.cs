using API.Contracts.Auth;
using API.Enum;
using API.Service;
using Application.BusinessProfileActions;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : BaseApiController
{
    public AuthController(TokenService tokenService, IMapper mapper)
    {
        _tokenService = tokenService;
        _mapper = mapper;
    }
    
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseObject>> Login([FromBody] LoginDto loginDto)
    {
        var businessProfile = await Mediator.Send(new FindOneByEmail.Query{ Email = loginDto.Email });

        if (businessProfile.Value == null || businessProfile.Value.Password != loginDto.Password)
            return Unauthorized();
        
        var token = _tokenService.GenerateToken(businessProfile.Value.Id.ToString(), ProfileType.Business);

        // return new LoginUserResponseObject
        // {
        //     Id = businessProfile.Value.Id,
        //     Email = businessProfile.Value.Email,
        //     AccessToken = token
        // };

        var loginUserResponseObject = _mapper.Map<LoginUserResponseObject>(businessProfile.Value);
        loginUserResponseObject.AccessToken = token;
        return loginUserResponseObject;
        
    }
}