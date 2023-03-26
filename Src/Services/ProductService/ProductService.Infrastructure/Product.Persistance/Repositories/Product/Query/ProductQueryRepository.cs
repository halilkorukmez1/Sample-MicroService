using Product.Application.Interfaces.Repositories.Product.Query;
using Product.Persistance.Elasticsearch.Config;
using Product.Persistance.Elasticsearch.Constants;
using Product.Persistance.Repositories.Elasticsearch.Query;

namespace Product.Persistance.Repositories.Product.Query;
public class ProductQueryRepository : ElasticQueryRepository<Domain.Entities.Product>, IProductReadRepository
{
    public ProductQueryRepository(IElasticContextProvider elasticClient) : base(elasticClient)
    {
    }

    public override string indexName => IndexConstants.ProductIndexName;
}
