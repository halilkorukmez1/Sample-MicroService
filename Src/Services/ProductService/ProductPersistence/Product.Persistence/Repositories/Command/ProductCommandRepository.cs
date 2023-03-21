using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Product.Application.Repositories.Command;
using Product.Persistence.DataContext.MsSql;

namespace Product.Persistence.Repositories.Command
{
    public class ProductCommandRepository : IProductCommandRepository
    {
        readonly private ProductDataContext _context;
        public ProductCommandRepository(ProductDataContext context) => _context = context;

        public DbSet<Domain.Entities.Product> _set => _context.Set<Domain.Entities.Product>();

        public async Task<bool> AddAsync(Domain.Entities.Product model)
        {
            EntityEntry<Domain.Entities.Product> entityEntry = await _set.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var product = await _set.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (product != null)
            {
                EntityEntry<Domain.Entities.Product> entityEntry = _set.Remove(product);
                return entityEntry.State == EntityState.Deleted;
            }
            return false;
        }
        public bool Update(Domain.Entities.Product model)
        {
            EntityEntry<Domain.Entities.Product> entityEntry = _set.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
