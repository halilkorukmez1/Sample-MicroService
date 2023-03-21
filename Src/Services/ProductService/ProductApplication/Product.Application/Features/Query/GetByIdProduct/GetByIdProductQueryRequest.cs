using MediatR;

namespace Product.Application.Features.Query.GetByIdProduct;
public class GetByIdProductQueryRequest : IRequest<GetByIdProductQueryResponse>
{
    public required Guid Id { get; set; }
}