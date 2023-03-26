using Nest;
using Product.Application.Dtos;

namespace Product.Persistance.Elasticsearch.Results;
public class ResponseService
{
    public static ResponseDto<T> ReturnResult<T, ET>(T entity, ET elasticSearchResult) where ET : ResponseBase 
        => new ResponseDto<T>()
        {
            Data = entity,
            ErrorMessage = elasticSearchResult.ServerError?.Error?.Reason,
            IsValid = elasticSearchResult.IsValid,
            StatusCode = (int)elasticSearchResult.ApiCall.HttpStatusCode,
            Success = elasticSearchResult.ApiCall.Success
        };
}