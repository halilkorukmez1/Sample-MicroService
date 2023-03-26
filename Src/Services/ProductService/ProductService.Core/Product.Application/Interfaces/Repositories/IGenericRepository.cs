using Product.Domain.Common;

namespace Product.Application.Interfaces.Repositories;
public interface IGenericRepository<T> : IRepository<T> where T : BaseEntity
{
    Task<bool> AddAsync(T model);
    Task<bool> RemoveAsync(string id);
    bool Update(T model);
    Task<bool> SaveAsync(T model);
}
