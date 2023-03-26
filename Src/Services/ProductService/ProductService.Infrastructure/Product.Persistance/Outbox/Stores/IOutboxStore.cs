namespace Product.Infrastructure.Outbox.Stores;
public interface IOutboxStore
{
    Task Add(OutboxMessage message);
    Task<IEnumerable<Guid>> GetUnprocessedMessageIds();
    Task SetMessageToProcessed(Guid id);
    Task Delete(List<Guid> ids);
    Task<OutboxMessage> GetMessage(Guid id);
}
