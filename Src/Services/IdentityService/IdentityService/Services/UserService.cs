using IdentityService.Config;
using IdentityService.Data.Context;
using IdentityService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Services;

public class UserService : IUserService
{
    private readonly IIdentityContext _context;
    private readonly AppSettings _appSettings;
    public UserService(IIdentityContext context, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public async Task<List<User>> GetAll() 
        => await _context.Users.Find(x => x.IsActive).ToListAsync();

    public async Task<User> GetById(string id) 
        => await _context.Users.Find(x => x.Id == Guid.Parse(id)).FirstOrDefaultAsync();

    public async Task<string> LoginAsync(string userName, string password)
    {  
        var user = await _context.Users.Find(x => x.Username == userName && x.Password == password).FirstOrDefaultAsync();
        return GenerateJwtToken(user);
    }
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            { 
                new Claim("id", user.Id.ToString()),
                new Claim("userName",user.Username),
                new Claim("lastName",user.LastName),
                new Claim("firsName",user.FirstName),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}