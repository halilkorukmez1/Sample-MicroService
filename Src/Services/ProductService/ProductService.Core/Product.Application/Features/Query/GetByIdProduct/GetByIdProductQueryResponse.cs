namespace Product.Application.Features.Query.GetByIdProduct;
public class GetByIdProductQueryResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required DateTime CreatedDate { get; set; }
}