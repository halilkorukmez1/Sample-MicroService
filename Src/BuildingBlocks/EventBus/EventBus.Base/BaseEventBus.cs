using EventBus.Base.Abstraction;
using EventBus.Base.Config;
using EventBus.Base.Events;
using EventBus.Base.SubscriptionManager;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBus.Base;
public abstract class BaseEventBus : IEventBus
{
    public readonly IServiceProvider _serviceProvider;
    public readonly IEventBusSubscriptionManager _subsManager;
    public EventBusConfig EventBusConfig { get; set; }
    protected BaseEventBus(EventBusConfig eventBusConfig, IServiceProvider serviceProvider)
    {
        EventBusConfig = eventBusConfig;
        _serviceProvider = serviceProvider;
        _subsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }

    public virtual string ProcessEventName(string eventName)
        => EventBusConfig.DeleteEventPrefix
           ? eventName.TrimStart(EventBusConfig.EventNamePrefix.ToArray())
           : EventBusConfig.DeleteEventSuffix
           ? eventName.TrimEnd(EventBusConfig.EventNameSuffix.ToArray())
           : eventName;

    public async Task<bool> ProcessEvent(string eventName, string message)
    {
        if (_subsManager.IsSubscriptionsForEvent(ProcessEventName(eventName)))
        {
            var subscriptions = _subsManager.GetHandlersForEvent(eventName);
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                foreach (var subscription in subscriptions)
                {
                    var handler = _serviceProvider.GetService(subscription.HandlerType);
                    if (handler == null) continue;
                    var eventType = _subsManager.GetEventTypeByName($"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}");
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
            return true;
        }
        return false;
    }

    public abstract void Publish(IntegrationEvent @event);
    public abstract void Subscribe<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
    public abstract void UnSubscribe<TEvent, TEventHandler>() where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>;
    public virtual string GetSubName(string eventName) => $"{EventBusConfig.SubscriberServiceName}.{ProcessEventName(eventName)}";
    public virtual void Dispose()
    {
        EventBusConfig = null;
        _subsManager.Clear();
    }
}