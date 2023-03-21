using EventBus.Base.Abstraction;
using EventBus.Base.Config;
using EventBus.Base.Enum;
using EventBus.RabbitMq;

namespace EventBus.Manager;
public static class EventBusManager
{
    public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        => config.EventBusType switch
        {
            EventBusType.RabbitMQ
            => new RabbitMQManager(config, serviceProvider)
        };
}