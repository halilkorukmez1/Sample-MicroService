namespace BasketService.Dtos;
public class BasketDto
{
    public required string UserId { get; set; }
    public required string DiscountCode { get; set; }
    public List<BasketItemDto> BasketItems { get; set; }
    public decimal? TotalPrice { get => BasketItems?.Sum(x => x.Price * x.Quantity); }
}
