namespace Manuela.AppRouting;

public class Route(Type type, string? name = null, bool isSingleton = false)
{
    public static Route Empty { get; } = new(typeof(object), "{empty}", false);

    public string Name { get; set; } = name ?? type.Name;
    public Type ViewType { get; set; } = type;
    public bool IsSingleton { get; set; } = isSingleton;
}

public class Route<T>(string? name = null, bool isSingleton = false)
    : Route(typeof(T), name, isSingleton)
        where T : ContentView
{ }
