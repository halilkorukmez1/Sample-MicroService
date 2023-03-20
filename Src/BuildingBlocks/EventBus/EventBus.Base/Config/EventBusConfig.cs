using EventBus.Base.Enum;

namespace EventBus.Base.Config;
public class EventBusConfig
{
    public string DefaultTopicName { get; set; } = "Sample";
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public int Port { get; set; }
    public string SubscriberServiceName { get; set; } = String.Empty;
    public string EventNamePrefix { get; set; } = String.Empty;
    public string EventNameSuffix { get; set; } = "Event";
    public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;
    public object Connection { get; set; }
    public bool DeleteEventPrefix => !String.IsNullOrEmpty(EventNamePrefix);
    public bool DeleteEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);
}
