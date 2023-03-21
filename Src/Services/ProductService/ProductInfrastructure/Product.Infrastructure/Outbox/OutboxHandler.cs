using MediatR;
using Newtonsoft.Json;

namespace Product.Infrastructure.Outbox;
public class OutboxHandler : IRequestHandler<CreateOutboxMessageEvent,bool>
{
    private readonly IOutboxListener _outboxListener;
    public OutboxHandler(IOutboxListener outboxListener) => _outboxListener = outboxListener;
    public async Task<bool> Handle(CreateOutboxMessageEvent request, CancellationToken cancellationToken)
    {
        await _outboxListener.Commit(new OutboxMessage
        {
            Type = "Product",
            Data = request != null
                         ? JsonConvert.SerializeObject(request, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                         : "{}"
        });
        return true;
    }
}
