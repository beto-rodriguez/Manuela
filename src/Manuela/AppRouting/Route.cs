namespace Manuela.AppRouting;

public class Route(Type type, string? routeName = null, bool isSingleton = false)
{
    public static Route Empty { get; } = new(typeof(object), "{empty}", false);
    public string RouteName { get; set; } = routeName ?? type.Name;
    public Type ViewType { get; set; } = type;
    public bool IsSingleton { get; set; } = isSingleton;
}

public class Route<T>(string? routeName = null, bool isSingleton = false)
    : Route(typeof(T), routeName, isSingleton)
        where T : ContentView
{ }
