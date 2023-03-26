using Nest;
using Product.Application.Interfaces.Repositories.Elasticsearch.Query;
using Product.Domain.Common;
using Product.Persistance.Elasticsearch.Config;

namespace Product.Persistance.Repositories.Elasticsearch.Query;
public abstract class ElasticQueryRepository<T> : IElasticQueryRepository<T> where T : BaseEntity
{
    private readonly IElasticClient _elasticClient;
    public abstract string indexName { get; }
    public ElasticQueryRepository(IElasticContextProvider elasticClient)
        => _elasticClient = elasticClient.GetClient();

    public async Task<T> GetDocumentByIdAsync(Guid id)
    {
        var result = await _elasticClient.GetAsync<T>(id, g => g.Index(indexName));
        return result.Source;
    }
    public async Task<IList<T>> GetDocumentListAsync(QueryContainer query)
    {
        var result = await _elasticClient.SearchAsync<T>(s => s.Index(indexName).Query(_ => query).MatchAll().TrackTotalHits(true));
        return result.Documents.ToList();
    }
    public async Task<IList<T>> GetDocumentListAsync()
    {
        var result = await _elasticClient.SearchAsync<T>(s => s.Index(indexName).MatchAll().TrackTotalHits(true));
        return result.Documents.ToList();
    }
    public async Task<List<T>> GetDocumentManyByIdAsync(List<long> list)
    {
        var result = await _elasticClient.GetManyAsync<T>(list, indexName);
        return result.Select(x => x.Source).ToList();
    }
   
}