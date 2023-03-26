using MediatR;
using Product.Application.Interfaces.Repositories.Product.Command;

namespace Product.Application.Features.Command.CreateProduct;
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductWriteRepository _productWriteRepository;
    public CreateProductCommandHandler(IProductWriteRepository productWriteRepository)
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
