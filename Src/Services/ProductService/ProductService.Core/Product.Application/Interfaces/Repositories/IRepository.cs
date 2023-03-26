using Microsoft.EntityFrameworkCore;
using Product.Domain.Common;

namespace Product.Application.Interfaces.Repositories;
public interface IRepository<T> where T : BaseEntity
{
    DbSet<T> _set { get; }
}
