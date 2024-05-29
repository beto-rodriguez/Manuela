namespace Manuela.AppRouting;

public class Route(
    Type type,
    Type? viewModelType = null,
    string? routeName = null,
    bool isModal = false,
    bool isSingleton = false)
{
    public static Route Empty { get; } = new(typeof(object), null, "{empty}", false);
    public string RouteName { get; } = routeName ?? type.Name;
    public Type ViewType { get; } = type;
    public Type? ViewModelType { get; } = viewModelType;
    public bool IsModal { get; } = isModal;
    public bool IsSingleton { get; } = isSingleton;

    public Dictionary<string, string>? Parameters { get; set; }

    public bool IsEmpty => RouteName == Empty.RouteName;
}

public class Route<TView>(string? routeName = null, bool isSingleton = false)
    : Route(typeof(TView), null, routeName, isSingleton)
        where TView : ContentView
{ }


public class Route<TView, TViewModel>(string? routeName = null, bool isSingleton = false)
    : Route(typeof(TView), typeof(TViewModel), routeName, isSingleton)
        where TView : ContentView
{ }
