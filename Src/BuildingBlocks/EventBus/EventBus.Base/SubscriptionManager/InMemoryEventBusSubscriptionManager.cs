using EventBus.Base.Abstraction;
using EventBus.Base.Events;

namespace EventBus.Base.SubscriptionManager;
public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<SubscriptionDetail>> _handlers;
    private readonly List<Type> _eventTypes;
    public event EventHandler<string> OnEventRemoved;
    public Func<string, string> _eventNameGetter;
    public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
    {
        _handlers = new Dictionary<string, List<SubscriptionDetail>>();
        _eventTypes = new List<Type>();
        _eventNameGetter = eventNameGetter;
    }
    public bool IsEmpty => !_handlers.Keys.Any();
    public void Clear() => _handlers.Clear();
    public void AddSubscription<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>
    {
        AddSubscription(typeof(TEventHandler), GetEventKey<TEvent>());
        if (!_eventTypes.Contains(typeof(TEvent))) _eventTypes.Add(typeof(TEvent));
    }
    private void AddSubscription(Type handlerType, string eventName)
    {
        if (!IsSubscriptionsForEvent(eventName)) _handlers.Add(eventName, new List<SubscriptionDetail>());
        if (_handlers[eventName].Any(s => s.HandlerType == handlerType)) throw new ArgumentException(nameof(handlerType));
        _handlers[eventName].Add(SubscriptionDetail.Typed(handlerType));
    }
    public void RemoveSubscription<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>
        => RemoveHandler(GetEventKey<TEvent>(), FindSubscriptionToRemove<TEvent, TEventHandler>());
    private void RemoveHandler(string eventName, SubscriptionDetail subsToRemove)
    {
        if (subsToRemove != null && !_handlers[eventName].Any())
        {
            _handlers.Remove(eventName);
            var eventType = _eventTypes.SingleOrDefault(x => x.Name == eventName);
            if (eventType != null) _eventTypes.Remove(eventType);
            RaiseOnEventRemoved(eventName);
        }
    }
    public IEnumerable<SubscriptionDetail> GetHandlersForEvent<TEvent>() where TEvent : IntegrationEvent => GetHandlersForEvent(GetEventKey<TEvent>());
    public IEnumerable<SubscriptionDetail> GetHandlersForEvent(string eventName) => _handlers[eventName];
    private void RaiseOnEventRemoved(string eventName) => OnEventRemoved?.Invoke(this, eventName);
    private SubscriptionDetail FindSubscriptionToRemove<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>
        => FindSubscriptionToRemove(GetEventKey<TEvent>(), typeof(TEventHandler));
    private SubscriptionDetail FindSubscriptionToRemove(string eventName, Type handlerType)
        => IsSubscriptionsForEvent(eventName) ? _handlers[eventName].SingleOrDefault(x => x.HandlerType == handlerType) : null;
    public bool IsSubscriptionsForEvent<TEvent>() where TEvent : IntegrationEvent => IsSubscriptionsForEvent(GetEventKey<TEvent>());
    public bool IsSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);
    public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(x => x.Name == eventName);
    public string GetEventKey<TEvent>() => _eventNameGetter(typeof(TEvent).Name);
}