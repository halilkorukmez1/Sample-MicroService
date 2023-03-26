using System.ComponentModel.DataAnnotations;

namespace Product.Infrastructure.Outbox;
public class OutboxMessage
{
    public OutboxMessage(){ }
    public OutboxMessage(Guid id) => Id = id;
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; private set; } = DateTime.Now;
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime? Processed { get; set; }
    public bool? Status { get; set; }
    [Range(0,5)]
    public int Retry { get; set; }
}