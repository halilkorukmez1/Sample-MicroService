namespace Product.Infrastructure.Outbox;
public interface IOutboxListener
{
    Task Commit(OutboxMessage message);
}
