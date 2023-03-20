using EventBus.Base.Events;

namespace EventBus.Base.Abstraction;
public interface IIntegrationEventHandler<TEvent> : IIntegrationEventHandler where TEvent : IntegrationEvent
{
    Task Handle(TEvent @event);
}
public interface IIntegrationEventHandler { }
