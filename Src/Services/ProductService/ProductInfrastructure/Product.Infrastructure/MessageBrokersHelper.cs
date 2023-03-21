namespace Product.Infrastructure;
public static class MessageBrokersHelper
{
    public static string GetTypeName(Type type)
    {
        var name = type.FullName.ToLower().Replace("+", ".");
        if (type is IEvent) name += "_event";
        return name;
    }
    public static string GetTypeName<T>() => GetTypeName(typeof(T));
}
