namespace EventBus.Base.SubscriptionManager;
public class SubscriptionDetail
{
    public Type HandlerType { get; }
    public SubscriptionDetail(Type handlerType) => HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
    public static SubscriptionDetail Typed(Type handlerType) => new SubscriptionDetail(handlerType);
}