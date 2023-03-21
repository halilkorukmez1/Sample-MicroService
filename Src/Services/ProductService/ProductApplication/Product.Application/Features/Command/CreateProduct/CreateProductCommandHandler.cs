using MediatR;

namespace Product.Application.Features.Command.CreateProduct;
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    public CreateProductCommandHandler()
    {

    }
    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        return new();
    }
}
