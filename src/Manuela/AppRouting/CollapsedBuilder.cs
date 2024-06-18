namespace Manuela.AppRouting;

public class CollapsedBuilder
{
    internal List<Route> _routes = [];

    public CollapsedBuilder Add(
        Type viewType,
        Type? viewModelType,
        string routeName,
        string displayName,
        string icon,
        List<string>? styleClass = null)
    {
        _routes.Add(new Route(viewType, viewModelType, routeName, s =>
        {
            s.DisplayName = displayName;
            s.Icon = icon;
            s.StyleClass = styleClass;
        }));

        return this;
    }
}
