using EventBus.Base.Abstraction;

namespace Product.Infrastructure.Events.OutboxSendEvent
{
    public class WriteOutboxToElasticEventHandler : IIntegrationEventHandler<WriteOutboxToElasticEvent>
    {
        public async Task Handle(WriteOutboxToElasticEvent @event)
        {

        }
    }
}
