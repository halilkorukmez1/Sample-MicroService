using MediatR;
using Nest;
using Product.Application.Interfaces.Repositories.Product.Query;

namespace Product.Application.Features.Query.GetAllProduct;
public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    private readonly IProductReadRepository _repository;
    public GetAllProductQueryHandler(IProductReadRepository repository) => _repository = repository;

    public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetDocumentListAsync(new QueryContainerDescriptor<Domain.Entities.Product>().Terms(x => x.Field(x => x.IsActive).Terms(request.IsActive)));
        return new GetAllProductQueryResponse { Products = result, TotalProductCount = result.Count};
    }
}