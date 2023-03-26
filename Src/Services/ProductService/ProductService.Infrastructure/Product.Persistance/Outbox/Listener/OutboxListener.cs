using Product.Infrastructure.Outbox;
using Product.Infrastructure.Outbox.Stores;

namespace Product.Persistance.Outbox.Listener;
public class OutboxListener : IOutboxListener
{
    private readonly IOutboxStore _store;

    public OutboxListener(IOutboxStore store) => _store = store;

    public virtual async Task Commit(OutboxMessage message) => await _store.Add(message);
}
