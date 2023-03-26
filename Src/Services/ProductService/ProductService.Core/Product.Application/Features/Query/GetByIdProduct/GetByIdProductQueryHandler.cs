using MediatR;
using Product.Application.Interfaces.Repositories.Product.Query;

namespace Product.Application.Features.Query.GetByIdProduct;
public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
{
    private readonly IProductReadRepository _repository;
    public GetByIdProductQueryHandler(IProductReadRepository repository) 
        => _repository = repository;

    public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetDocumentByIdAsync(request.Id);
        return new GetByIdProductQueryResponse { Id = result.Id , Name = result.Name , Price = result.Price , CreatedDate = result.CreatedDate}; 
    }
}
