using Manuela.AppRouting;
using MauiIcons.SegoeFluent;

namespace SideMenuMauiApp.Layout;

public class AppRoutes
{
    private AppRoutes() { }

    private static readonly AppRoutes s_instance = new();

    private static List<Route> AllRoutes { get; } = [];
    public static List<IMenuItem> MainMenu { get; } = [];

    /// <summary>
    /// Build the application routes and configures the menu appeareance.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static Route[] Build(Action<AppRoutes> builder)
    {
        builder(s_instance);

        return AllRoutes
            .DistinctBy(x => x.RouteName)
            .ToArray();
    }

    /// <summary>
    /// Adds the route to the main menu, this menu should have no more than 5 elements.
    /// </summary>
    /// <typeparam name="T">The type of the route.</typeparam>
    /// <param name="icon">The icon.</param>
    /// <param name="displayName">The dipslay name, if null the name of the type will be used.</param>
    /// <param name="routeName">The route name, if null the name of the type will be used.</param>
    /// <param name="isSingleton">Indicates wether the view is a singleton.</param>
    public AppRoutes Add<T>(
        SegoeFluentIcons icon,
        string? displayName,
        string? routeName = null,
        IList<string>? styleClass = null,
        bool isSingleton = false)
            where T : ContentView
    {
        var defaultName = typeof(T).Name;

        MainMenu.Add(new IconAppRoute<T>(icon, displayName ?? defaultName, defaultName, isSingleton));
        AllRoutes.Add(new Route<T>(routeName, isSingleton));

        return s_instance;
    }

    /// <summary>
    /// Adds a route to the app, this route will not be visible in the menu, but is accessible through the navigation stack.
    /// </summary>
    /// <typeparam name="T">The type of the route.</typeparam>
    /// <param name="icon">The icon.</param>
    /// <param name="displayName">The dipslay name, if null the name of the type will be used.</param>
    /// <param name="routeName">The route name, if null the name of the type will be used.</param>
    /// <param name="isSingleton">Indicates wether the view is a singleton.</param>
    public AppRoutes AddHidden<T>(
        string? routeName = null,
        bool isSingleton = false)
            where T : ContentView
    {
        var defaultName = typeof(T).Name;
        AllRoutes.Add(new Route<T>(routeName ?? defaultName, isSingleton));

        return s_instance;
    }

    private class IconAppRoute<T>(
        SegoeFluentIcons icon, string name, string routeName, bool isSingleton)
            : Route(typeof(T), routeName, isSingleton), IMenuItem
                where T : ContentView
    {
        public SegoeFluentIcons Icon { get; } = icon;
        public string Display { get; } = name;
    }
}
