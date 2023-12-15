using API.Contracts.Auth;
using API.Enum;
using API.Service;
using Application.BusinessProfileActions;
using Application.Core.Security;
using AutoMapper;
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
}