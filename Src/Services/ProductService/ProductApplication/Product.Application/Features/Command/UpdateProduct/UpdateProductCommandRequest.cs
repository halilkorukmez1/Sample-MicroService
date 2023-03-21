using MediatR;

namespace Product.Application.Features.Command.UpdateProduct;
public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
