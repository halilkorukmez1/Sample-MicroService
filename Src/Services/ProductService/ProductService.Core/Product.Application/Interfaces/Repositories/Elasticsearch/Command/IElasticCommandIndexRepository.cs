using Product.Application.Dtos;

namespace Product.Application.Interfaces.Repositories.Elasticsearch.Command;
public interface IElasticCommandIndexRepository<T> where T : class
{
    Task<bool> CreateIndexAsync();
    Task<bool> DeleteIndexAsync();
}
