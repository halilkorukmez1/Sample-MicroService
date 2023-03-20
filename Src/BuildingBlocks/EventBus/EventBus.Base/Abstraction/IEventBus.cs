using EventBus.Base.Events;

namespace EventBus.Base.Abstraction;
public interface IEventBus : IDisposable
{
    void Publish(IntegrationEvent @event);
    void Subscribe<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
    void UnSubscribe<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
}
