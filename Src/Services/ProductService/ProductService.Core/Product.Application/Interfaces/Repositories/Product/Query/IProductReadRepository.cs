using Product.Application.Interfaces.Repositories.Elasticsearch.Query;

namespace Product.Application.Interfaces.Repositories.Product.Query
{
    public interface IProductReadRepository : IElasticQueryRepository<Domain.Entities.Product>
    {
    }
}
