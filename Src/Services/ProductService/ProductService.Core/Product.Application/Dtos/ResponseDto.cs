namespace Product.Application.Dtos;
public class ResponseDto<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; } = null;
    public bool IsValid { get; set; }
    public string? ServiceMessage { get; set; }
}