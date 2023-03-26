using Microsoft.EntityFrameworkCore;

namespace Product.Persistence.DataContext.MsSql;
public class ProductDataContext : DbContext
{
    public ProductDataContext(DbContextOptions<ProductDataContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
              optionsBuilder
                  .UseSqlServer(@"Connection String")
                  .EnableSensitiveDataLogging(false)
                  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        base.OnConfiguring(optionsBuilder);
    }
}
