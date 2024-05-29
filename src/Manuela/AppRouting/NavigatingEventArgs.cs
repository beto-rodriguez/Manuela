namespace Manuela.AppRouting;

public class NavigatingEventArgs(View? sender, Route oldRoute, Route newRoute)
{
    public View? Sender { get; } = sender;
    public Route OldRoute { get; } = oldRoute;
    public Route NewRoute { get; } = newRoute;

    public bool Navigating(string from, string to)
    {
        return OldRoute.RouteName == from && NewRoute.RouteName == to;
    }
}
