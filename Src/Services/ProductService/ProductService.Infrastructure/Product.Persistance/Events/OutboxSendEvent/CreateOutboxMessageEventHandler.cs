using MediatR;
using Newtonsoft.Json;
using Product.Infrastructure.Outbox;
using Product.Persistance.Outbox.Listener;

namespace Product.Persistance.Events.OutboxSendEvent;
public class CreateOutboxMessageEventHandler : IRequestHandler<CreateOutboxMessageEvent, bool>
{
    private readonly IOutboxListener _outboxListener;
    public CreateOutboxMessageEventHandler(IOutboxListener outboxListener) => _outboxListener = outboxListener;
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