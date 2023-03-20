using IdentityService.Models;
using MongoDB.Driver;

namespace IdentityService.Data.Seed;
public class IdentityContextSeed
{
    public static async Task SeedData(IMongoCollection<User> userCollection)
    {
        var exist = userCollection.Find(p => true).Any();
        if (!exist)  await userCollection.InsertManyAsync(GetPreconfiguredUsers());
    }

    private static IList<User> GetPreconfiguredUsers() 
        => new List<User>()
            {
                new User()
                {
                   CreatedDate= DateTime.Now,
                   FirstName = "Test",
                   Id= Guid.NewGuid(),
                   LastName= "Test",
                   Password= "Test",
                   Username= "Test",
                   IsActive= true,
                }
            };
}