using IdentityService.Config.DbSettings;
using IdentityService.Data.Context;
using IdentityService.Data.Seed;
using IdentityService.Models;
using MongoDB.Driver;

namespace IdentityService.Data;
public class IdentityContext : IIdentityContext
{
    public IdentityContext(IIdentityDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        Users = database.GetCollection<User>(nameof(User));

        IdentityContextSeed.SeedData(Users).Wait();

    }
    public IMongoCollection<User> Users { get; }

}