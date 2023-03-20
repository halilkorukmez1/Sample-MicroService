using Newtonsoft.Json;

namespace EventBus.Base.Events;
public class IntegrationEvent
{
    [JsonProperty]
    public Guid Id { get; private set; }
    [JsonProperty]
    public DateTime CreatedDate { get; private set; }

    [JsonConstructor]
    public IntegrationEvent(DateTime createdDate, Guid id)
    {
        CreatedDate = createdDate;
        Id = id;
    }
    public IntegrationEvent()
    {
        CreatedDate = DateTime.Now;
        Id = Guid.NewGuid();
    }
}