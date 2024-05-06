using Manuela.AppRouting;
using MauiIcons.SegoeFluent;

namespace ManuelaAppTemplate.AppLayout;

public class AppRoutes
{
    private AppRoutes() { }

    private static readonly AppRoutes s_instance = new();

    private static List<Route> AllRoutes { get; } = [];
    public static List<IMenuItem> MainMenu { get; } = [];
    public static List<IMenuItem> SecondaryMenu { get; } = [];

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
    public AppRoutes AddMain<T>(
        SegoeFluentIcons icon,
        string? displayName,
        string? routeName = null,
        IList<string>? styleClass = null,
        bool isSingleton = false)
            where T : ContentView
    {
        var defaultName = typeof(T).Name;

        MainMenu.Add(new IconAppRoute<T>(icon, displayName ?? defaultName, defaultName, styleClass, isSingleton));
        AllRoutes.Add(new Route<T>(routeName, isSingleton));

        return s_instance;
    }

    /// <summary>
    /// Adds the route to the secondary menu, this menu is visible on medium or greater screens,
    /// on smaller screens it will be placed on the more options menu.
    /// </summary>
    /// <typeparam name="T">The type of the route.</typeparam>
    /// <param name="icon">The icon.</param>
    /// <param name="displayName">The dipslay name, if null the name of the type will be used.</param>
    /// <param name="routeName">The route name, if null the name of the type will be used.</param>
    /// <param name="isSingleton">Indicates wether the view is a singleton.</param>
    public AppRoutes AddSecondary<T>(
        SegoeFluentIcons icon,
        string? displayName,
        string? routeName = null,
        IList<string>? styleClass = null,
        bool isSingleton = false)
            where T : ContentView
    {
        var defaultName = typeof(T).Name;

        SecondaryMenu.Add(new IconAppRoute<T>(icon, displayName ?? defaultName, defaultName, styleClass, isSingleton));
        AllRoutes.Add(new Route<T>(routeName, isSingleton));

        return s_instance;
    }

    /// <summary>
    /// Adds a collection of routes to the more options menu, this menu is collapsed and expanded when
    /// the user clicks on the more options button.
    /// </summary>
    /// <typeparam name="T">The type of the route.</typeparam>
    /// <param name="icon">The icon.</param>
    /// <param name="displayName">The dipslay name, if null the name of the type will be used.</param>
    /// <param name="routeName">The route name, if null the name of the type will be used.</param>
    /// <param name="isSingleton">Indicates wether the view is a singleton.</param>
    public AppRoutes AddCollapsed(
        SegoeFluentIcons icon,
        string displayName,
        Action<CollapsedBuilder> builder,
        IList<string>? styleClass = null)
    {
        var cb = new CollapsedBuilder();
        builder(cb);

        MainMenu.Add(new IconAppRoute<ContentView>(icon, displayName, "", styleClass, false)
        {
            CollapsedOptions = cb.Options
        });

        // Do not add the route to the AllRoutes collection.
        // this will ignore the route in the navigation stack.
        //AllRoutes.Add(new Route<T>(ro[uteName, isSingleton));

        return s_instance;
    }

    private class IconAppRoute<T>(
        SegoeFluentIcons icon, string name, string routeName, IList<string>? styleClass, bool isSingleton)
            : Route(typeof(T), routeName, isSingleton), IMenuItem
                where T : ContentView
    {
        public SegoeFluentIcons Icon { get; } = icon;
        public string Display { get; } = name;
        public bool IsCollapseButton => CollapsedOptions.Count > 0;
        public List<IMenuItem> CollapsedOptions { get; set; } = [];
        public AppCollapsedMenu? CollapsedMenu { get; set; }
        public IList<string>? StyleClass { get; } = styleClass;
    }

    public class CollapsedBuilder
    {
        public List<IMenuItem> Options { get; } = [];

        /// <summary>
        /// Adds a route to the more options menu.
        /// </summary>
        /// <typeparam name="T">The type of the route.</typeparam>
        /// <param name="icon">The icon.</param>
        /// <param name="displayName">The dipslay name, if null the name of the type will be used.</param>
        /// <param name="routeName">The route name, if null the name of the type will be used.</param>
        /// <param name="isSingleton">Indicates wether the view is a singleton.</param>
        public CollapsedBuilder Add<T>(
            SegoeFluentIcons icon,
            string? displayName,
            string? routeName = null,
            IList<string>? styleClass = null,
            bool isSingleton = false)
            where T : ContentView
        {
            var defaultName = typeof(T).Name;

            Options.Add(new IconAppRoute<T>(icon, displayName ?? defaultName, defaultName, styleClass, isSingleton));
            AllRoutes.Add(new Route<T>(routeName, isSingleton));

            return this;
        }
    }
}
