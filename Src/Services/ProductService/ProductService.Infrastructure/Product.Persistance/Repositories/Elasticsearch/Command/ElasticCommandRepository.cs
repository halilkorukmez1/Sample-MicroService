using Nest;
using Product.Application.Dtos;
using Product.Application.Interfaces.Repositories.Elasticsearch.Command;
using Product.Domain.Common;
using Product.Persistance.Elasticsearch.Config;
using Product.Persistance.Elasticsearch.Results;

namespace Product.Persistance.Repositories.Elasticsearch.Command;
public abstract class ElasticCommandRepository<T> : IElasticCommandRepository<T>, IElasticCommandIndexRepository<T> where T : BaseEntity
{
    private readonly IElasticClient _elasticClient;
    public abstract string indexName { get; }
    public ElasticCommandRepository(IElasticContextProvider elasticClient) 
        => _elasticClient = elasticClient.GetClient();

    public async Task<ResponseDto<T>> InsertDocumentAsync(T model)
        => ResponseService.ReturnResult(model, await _elasticClient.IndexAsync(model, i => i.Index(indexName)));

    public async Task<ResponseDto<IList<T>>> InsertDocumentManyAsync(IList<T> tList)
        => ResponseService.ReturnResult(tList, await _elasticClient.IndexManyAsync(tList, index: indexName));

    public async Task<ResponseDto<T>> UpdateDocumentAsync(T model)
        => ResponseService.ReturnResult(model, await _elasticClient.UpdateAsync<T>(model.Id, u => u.Index(indexName).Doc(model)));

    public async Task<ResponseDto<Guid>> DeleteDocumentByIdAsync(Guid id)
        => ResponseService.ReturnResult(id, await _elasticClient.DeleteAsync<T>(id, d => d.Index(indexName)));

    public async Task<bool> CreateIndexAsync()
    {
        var anyIndex = await _elasticClient.Indices.ExistsAsync(indexName);
        var result = !anyIndex.Exists
            ? await _elasticClient.Indices.CreateAsync(indexName, request
                       => request
                           .Index(indexName)
                           .Map<T>(m => m.AutoMap())
                           .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1)))
            : null;

        return result != null ? result.ApiCall.Success : false;

    }
    public async Task<bool> DeleteIndexAsync()
    {
        var result = await _elasticClient.Indices.DeleteAsync(indexName);
        return result.ApiCall.Success;
    }
}