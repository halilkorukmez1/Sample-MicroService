using IdentityService.Models;

namespace IdentityService.Services;
public interface IUserService
{
    Task<List<User>> GetAll();
    Task<User> GetById(string id);
    Task<string> LoginAsync(string userName, string password);
}
