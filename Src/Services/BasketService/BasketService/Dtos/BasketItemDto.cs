namespace BasketService.Dtos;
public class BasketItemDto
{
    public required short Quantity { get; set; }
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
}
