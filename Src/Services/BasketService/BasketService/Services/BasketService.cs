using BasketService.Config.Connections;
using BasketService.Dtos;
using Newtonsoft.Json;

namespace BasketService.Services;
public class BasketService : IBasketService
{
    private readonly IRedisConnectionFactory _redis;
    public BasketService(IRedisConnectionFactory redis) => _redis = redis;
    public async Task<bool> DeleteChanges(string userId) 
        => await _redis.GetDatabase().KeyDeleteAsync(userId);
    public async Task<bool> SubmitChanges(BasketDto basketDto) 
        => await _redis.GetDatabase().StringSetAsync(basketDto.UserId, JsonConvert.SerializeObject(basketDto));
    public async Task<BasketDto?> GetBasketByUserId(string userId)
    {
        var result = await _redis.GetDatabase().StringGetAsync(userId);
        return !string.IsNullOrEmpty(result)
            ? JsonConvert.DeserializeObject<BasketDto>(result)
        : null;
    }
}
