using IdentityService.Models;
using MongoDB.Driver;

namespace IdentityService.Data.Context;
public interface IIdentityContext
{
    IMongoCollection<User> Users { get; }
}