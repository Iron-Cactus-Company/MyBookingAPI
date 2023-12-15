using System.Text;
using API.Service;
using Application.Core.Security;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class AuthServiceExtensions
{
    //Add extension method for adding all needed services to builder
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        //the key used for generating tokens
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        services.AddScoped<TokenService>();
        services.AddScoped<IPasswordHasher<BusinessProfile>, PasswordHasher<BusinessProfile>>();
        services.AddScoped<PasswordManager>();

        return services;
    }
}