using MediatR;
using Product.Application.Repositories.Command;

namespace Product.Application.Features.Command.CreateProduct;
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductCommandRepository _productWriteRepository;
    public CreateProductCommandHandler(IProductCommandRepository productWriteRepository)
        => _productWriteRepository = productWriteRepository;

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var product = new Domain.Entities.Product
        {
            Name = request.Name,
            Price = request.Price
        };
        await _productWriteRepository.AddAsync(product);
        await _productWriteRepository.SaveAsync(product);
        return new();
    }
}
