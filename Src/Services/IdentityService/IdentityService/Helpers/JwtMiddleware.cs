using IdentityService.Config;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IdentityService.Helpers;
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token is not null) await AttachUserToContext(context, userService, token);

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            new JwtSecurityTokenHandler().ValidateToken(token, 
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            context.Items["User"] = new 
            { 
                Id = jwtToken.Claims.First(x => x.Type == "id").Value,
                LastName = jwtToken.Claims.First(x => x.Type == "lastName").Value,
                FirstName = jwtToken.Claims.First(x => x.Type == "firstName").Value,
                UserName = jwtToken.Claims.First(x => x.Type == "userName").Value,
            };
        }
        catch
        {
            // LOG
        }
    }
}