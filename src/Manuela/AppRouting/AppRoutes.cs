namespace Manuela.AppRouting;

public class AppRoutes
{
    private AppRoutes() { }

    internal static readonly AppRoutes s_instance = new();

    internal static List<Route> AllRoutes { get; } = [];
    public static IEnumerable<Route> MainMenu => AllRoutes
        .Where(x => x.Settings.IsMain);
    public static IEnumerable<Route> SecondaryMenu => AllRoutes
        .Where(x => !x.Settings.IsMain && !x.Settings.IsHidden);
    public static IEnumerable<Route> CollapsedElements => AllRoutes
        .Where(x => x.Settings.CollapsedRoutes is not null);

    public static Route[] Build(Action<AppRoutes> builder)
    {
        builder(s_instance);

        return AllRoutes
            .DistinctBy(x => x.RouteName)
            .Where(x => x.Settings.CollapsedRoutes is null)
            .ToArray();
    }

    public AppRoutes Add(
        string routeName,
        Type viewType,
        Type? viewModelType,
        Action<RouteMenuSettings>? menuSettings = null,
        bool isSingleton = false)
    {
        var routeSettings = new RouteMenuSettings();
        menuSettings?.Invoke(routeSettings);

        AllRoutes.Add(new Route(viewType, viewModelType, routeName, menuSettings, isSingleton));

        return s_instance;
    }

    public AppRoutes Add<TView>(
       string routeName,
       AppView<TView> viewSettings,
       Action<RouteMenuSettings>? menuSettings = null,
       bool isSingleton = false)
    {
        return Add(routeName, viewSettings.ViewType, null, menuSettings, isSingleton);
    }

    public AppRoutes Add<TView, TViewModel>(
        string routeName,
        AppView<TView, TViewModel> viewSettings,
        Action<RouteMenuSettings>? menuSettings = null,
        bool isSingleton = false)
    {
        return Add(routeName, viewSettings.ViewType, viewSettings.ViewModelType, menuSettings, isSingleton);
    }

    public AppRoutes AddCollapsed(
        string icon,
        string displayName,
        Action<CollapsedBuilder> builder,
        IList<string>? styleClass = null)
    {
        var cb = new CollapsedBuilder();
        builder(cb);

        var collapsableMenuItem = new Route(
            typeof(object), typeof(object), "{collapsed}", s => s.Main(icon, displayName), false);

        collapsableMenuItem.Settings.CollapsedRoutes = cb._routes;

        AllRoutes.Add(collapsableMenuItem);

        foreach (var route in cb._routes)
            AllRoutes.Add(route);

        return s_instance;
    }
}
