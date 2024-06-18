namespace Manuela.AppRouting;

public class Route
{
    public Route(
        Type viewType,
        Type? viewModelType = null,
        string? routeName = null,
        Action<RouteMenuSettings>? settings = null,
        bool isSingleton = false)
    {
        RouteName = routeName ?? viewType.Name;
        ViewType = viewType;
        ViewModelType = viewModelType;
        IsSingleton = isSingleton;
        Settings = new RouteMenuSettings();
        settings?.Invoke(Settings);
    }

    public static Route Empty { get; } = new(typeof(object), null, "{empty}", null, false);
    public string RouteName { get; }
    public Type ViewType { get; }
    public Type? ViewModelType { get; }
    public bool IsSingleton { get; }
    public RouteMenuSettings Settings { get; } = new();
    public Dictionary<string, string>? Parameters { get; internal set; }

    public bool IsEmpty => RouteName == Empty.RouteName;
}

public class Route<TView>(
    string? routeName = null,
    Action<RouteMenuSettings>? settings = null,
    bool isSingleton = false)
        : Route(typeof(TView), null, routeName, settings, isSingleton)
            where TView : ContentView
{ }

public class Route<TView, TViewModel>(
    string? routeName = null,
    Action<RouteMenuSettings>? settings = null,
    bool isSingleton = false)
        : Route(typeof(TView), typeof(TViewModel), routeName, settings, isSingleton)
            where TView : ContentView
{ }
