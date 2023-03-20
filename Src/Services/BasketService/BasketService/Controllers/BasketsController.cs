using BasketService.Dtos;
using BasketService.Helpers;
using BasketService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        public BasketsController(IBasketService basketService) 
            => _basketService = basketService;

        [CustomAuthorize]
        [HttpGet("GetBasketByUserId")]
        public async Task<IActionResult> GetBasketByUserIdAsync(string userId) 
            => Ok(await _basketService.GetBasketByUserIdAsync(userId));

        [CustomAuthorize]
        [HttpPost("SaveOrUpdate")]
        public async Task<IActionResult> SaveOrUpdateAsync([FromBody]BasketDto basketDto)
            => Ok(await _basketService.SubmitChangesAsync(basketDto));

        [CustomAuthorize]
        [HttpGet("Delete")]
        public async Task<IActionResult> DeleteBasketByUserId(string userId)
            => Ok(await _basketService.DeleteBasketAsync(userId));
    }
}
