using MediatR;

namespace Product.Persistance.Events.OutboxSendEvent;
public class CreateOutboxMessageEvent : IRequest<bool>
{
    public string Data { get; set; }
}

