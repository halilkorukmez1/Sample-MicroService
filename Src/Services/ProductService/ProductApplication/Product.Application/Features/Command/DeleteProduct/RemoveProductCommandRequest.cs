
using MediatR;

namespace Product.Application.Features.Command.DeleteProduct;
public class RemoveProductCommandRequest : IRequest<RemoveProductCommandResponse>
{
    public required Guid Id { get; set; }
}
