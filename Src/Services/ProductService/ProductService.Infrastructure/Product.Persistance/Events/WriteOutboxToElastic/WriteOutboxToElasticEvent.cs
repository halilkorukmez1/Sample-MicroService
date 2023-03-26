using EventBus.Base.Events;

namespace Product.Infrastructure.Events.OutboxSendEvent
{
    public class WriteOutboxToElasticEvent : IntegrationEvent
    {
        public string Data { get; }
        public WriteOutboxToElasticEvent(string data) => Data = data;
    }
}
