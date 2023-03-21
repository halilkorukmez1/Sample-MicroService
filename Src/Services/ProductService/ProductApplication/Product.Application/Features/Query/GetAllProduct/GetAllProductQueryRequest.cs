using MediatR;

namespace Product.Application.Features.Query.GetAllProduct;
public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
{
    public required bool IsActive { get; set; }
}
