using Nest;

namespace Product.Persistance.Elasticsearch.Config;
public interface IElasticContextProvider
{
    IElasticClient GetClient();
}