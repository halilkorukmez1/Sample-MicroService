using Nest;

namespace Product.Application.Interfaces.Repositories.Elasticsearch.Query;
public interface IElasticQueryRepository<T> where T : class
{
    Task<List<T>> GetDocumentManyByIdAsync(List<long> list);
    Task<IList<T>> GetDocumentListAsync(QueryContainer query);
    Task<T> GetDocumentByIdAsync(Guid id);
}