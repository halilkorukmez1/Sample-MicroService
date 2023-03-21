using MediatR;

namespace Product.Infrastructure.Outbox
{
    public class CreateOutboxMessageEvent : IRequest<bool>
    {
        public string Data { get; set; }
    }
}
