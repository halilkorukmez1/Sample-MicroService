using Product.Infrastructure.Outbox;

namespace Product.Persistance.Outbox.Listener;
public interface IOutboxListener
{
    Task Commit(OutboxMessage message);
}
