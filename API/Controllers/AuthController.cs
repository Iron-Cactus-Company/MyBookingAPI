using API.Contracts.Auth;
using API.Enum;
using API.Service;
using Application.BusinessProfileActions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : BaseApiController
{
    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    private readonly TokenService _tokenService;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserDto>> Login([FromBody] LoginDto loginDto)
    {
        var businessProfile = await Mediator.Send(new FindOneByEmail.Query{ Email = loginDto.Email });

        if (businessProfile.Value == null || businessProfile.Value.Password != loginDto.Password)
            return Unauthorized();
        
        var token = _tokenService.GenerateToken(businessProfile.Value.Id.ToString(), ProfileType.Business);

        return new LoginUserDto
        {
            Id = businessProfile.Value.Id,
            Email = businessProfile.Value.Email,
            AccessToken = token
        };
    }
}