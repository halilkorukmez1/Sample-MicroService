using Newtonsoft.Json;
using Product.Infrastructure.Outbox.Stores;

namespace Product.Infrastructure.Outbox;
public class OutboxListener : IOutboxListener
{
    private readonly IOutboxStore _store;

    public OutboxListener(IOutboxStore store) => _store = store;

    public virtual async Task Commit(OutboxMessage message) => await _store.Add(message);

}
