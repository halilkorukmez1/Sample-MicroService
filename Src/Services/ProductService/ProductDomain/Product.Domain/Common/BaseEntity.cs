namespace Product.Domain.Common;
public class BaseEntity
{
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required DateTime CreatedDate { get; set; } = DateTime.Now;
    public required bool IsActive { get; set; } = true;
    public DateTime? UpdatedDate { get; set; }
}
