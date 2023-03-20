using IdentityService.Helpers;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [CustomAuthorize]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync() => Ok(await _userService.GetAll());

    [CustomAuthorize]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetByIdAsync(Guid id) => Ok(await _userService.GetById(id.ToString())); 
    
    [HttpGet("GetToken")]
    public async Task<IActionResult> GetTokenAsync(string username, string password) => Ok(await _userService.LoginAsync(username,password));
}
