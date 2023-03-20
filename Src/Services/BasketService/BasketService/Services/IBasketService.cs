using BasketService.Dtos;

namespace BasketService.Services;
public interface IBasketService
{
    Task<BasketDto> GetBasketByUserId(string userId);
    Task<bool> SubmitChanges(BasketDto basketDto);
    Task<bool> DeleteChanges(string userId);
}