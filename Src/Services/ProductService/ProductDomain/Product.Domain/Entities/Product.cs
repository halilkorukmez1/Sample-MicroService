using Product.Domain.Common;

namespace Product.Domain.Entities;
public class Product : BaseEntity
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
