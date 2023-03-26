using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Product.Application.Interfaces.Repositories.Product.Command;
using Product.Persistance.Events.OutboxSendEvent;
using Product.Persistence.DataContext.MsSql;

namespace Product.Persistance.Repositories.Product.Command;
public class ProductCommandRepository : IProductWriteRepository
{
    private readonly IMediator _mediator;
    readonly private ProductDataContext _context;
    public ProductCommandRepository(ProductDataContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public DbSet<Domain.Entities.Product> _set => throw new NotImplementedException();

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

    public async Task<bool> SaveAsync(Domain.Entities.Product model)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var res = await _mediator.Send(new CreateOutboxMessageEvent { Data = JsonConvert.SerializeObject(model) });
                if (!res) throw new Exception();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }

    public bool Update(Domain.Entities.Product model)
    {
        EntityEntry<Domain.Entities.Product> entityEntry = _set.Update(model);
        return entityEntry.State == EntityState.Modified;
    }
}