namespace Manuela;

/// <summary>
/// Defines the route map for the application.
/// </summary>
public static class Routing
{
    public static Dictionary<string, Route> Routes { get; internal set; } = [];
    internal static IServiceCollection? ServiceCollection { get; set; }
    private static IServiceProvider? ServiceProvider { get; set; }
    public static Route ActiveRoute { get; internal set; } = Route.Empty;

    public static event Action<Route>? Navigating;

    public static void GoTo<T>() => GoTo(typeof(T).Name);

    public static void GoTo(string route)
    {
        if (ServiceProvider is null)
        {
            ServiceProvider = ServiceCollection?.BuildServiceProvider();
            ServiceCollection = null;
        }

        if (!Routes.TryGetValue(route, out var routeObject))
            throw new InvalidOperationException($"The route {route} does not exists.");

        var previousRoute = ActiveRoute;
        ActiveRoute = routeObject;

        var view = ServiceProvider?.GetRequiredService(routeObject.ViewType) as ContentView ??
            throw new InvalidOperationException($"The view type {routeObject.ViewType} is not assignable to ContentView type.");

        Navigating?.Invoke(previousRoute);

        AppPage.Current.Body = view;
    }
}
