namespace Product.Application.Repositories.Command;
public interface IProductCommandRepository
{
    Task<bool> AddAsync(Domain.Entities.Product model);
    Task<bool> RemoveAsync(string id);
    bool Update(Domain.Entities.Product model);
    Task<bool> SaveAsync(Domain.Entities.Product @event);
}
