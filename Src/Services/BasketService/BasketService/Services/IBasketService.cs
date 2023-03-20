using BasketService.Dtos;

namespace BasketService.Services;
public interface IBasketService
{
    Task<BasketDto> GetBasketByUserIdAsync(string userId);
    Task<bool> SubmitChangesAsync(BasketDto basketDto);
    Task<bool> DeleteBasketAsync(string userId);
}