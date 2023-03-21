using Product.Application.Features.Query.GetAllProduct;
using Product.Application.Features.Query.GetByIdProduct;

namespace Product.Application.Repositories.Query;
public interface IProductQueryRepository
{
    Task<List<GetAllProductQueryResponse>> GetAllAsync();
    Task<GetByIdProductQueryResponse> GetByIdAsync(string id);
}
