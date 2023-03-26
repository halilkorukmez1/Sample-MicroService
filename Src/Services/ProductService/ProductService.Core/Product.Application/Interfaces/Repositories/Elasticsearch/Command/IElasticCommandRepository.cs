using Product.Application.Dtos;

namespace Product.Application.Interfaces.Repositories.Elasticsearch.Command;
public interface IElasticCommandRepository<T> where T : class
{
    Task<ResponseDto<Guid>> DeleteDocumentByIdAsync(Guid id );
    Task<ResponseDto<T>> UpdateDocumentAsync(T model);
    Task<ResponseDto<IList<T>>> InsertDocumentManyAsync(IList<T> tList);
    Task<ResponseDto<T>> InsertDocumentAsync(T model);
}
