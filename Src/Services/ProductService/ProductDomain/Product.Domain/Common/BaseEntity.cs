namespace Product.Domain.Common;
public class BaseEntity
{
    public Guid Id { get; set; } 
    public DateTime CreatedDate { get; set; } 
    public bool IsActive { get; set; } 
    public DateTime? UpdatedDate { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
        IsActive = true;
    }
}
