using MediatR;

namespace Product.Application.Features.Command.CreateProduct;
public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}