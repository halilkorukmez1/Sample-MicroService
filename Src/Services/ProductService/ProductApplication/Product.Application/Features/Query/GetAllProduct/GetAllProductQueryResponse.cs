namespace Product.Application.Features.Query.GetAllProduct;
public class GetAllProductQueryResponse
{
    public required int TotalProductCount { get; set; }
    public required object Products { get; set; }
}
