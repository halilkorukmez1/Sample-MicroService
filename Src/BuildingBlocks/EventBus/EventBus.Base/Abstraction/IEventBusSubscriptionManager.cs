using EventBus.Base.Events;
using EventBus.Base.SubscriptionManager;

namespace EventBus.Base.Abstraction;
public interface IEventBusSubscriptionManager
{
    void AddSubscription<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
    void RemoveSubscription<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
    bool IsSubscriptionsForEvent<TEvent>() where TEvent : IntegrationEvent;
    bool IsSubscriptionsForEvent(string eventName);
    IEnumerable<SubscriptionDetail> GetHandlersForEvent<TEvent>() where TEvent : IntegrationEvent;
    IEnumerable<SubscriptionDetail> GetHandlersForEvent(string eventName);
    Type GetEventTypeByName(string eventName);
    string GetEventKey<TEvent>();
    void Clear();
    event EventHandler<string> OnEventRemoved;
    bool IsEmpty { get; }
}