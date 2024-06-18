namespace Manuela.AppRouting;

public class RouteMenuSettings
{
    public bool IsMain { get; set; }
    public bool IsHidden { get; set; } = true;
    public string? DisplayName { get; set; }
    public string? Icon { get; set; }
    public IList<string>? StyleClass { get; set; }
    public ICollapsibleMenu? CollapsedMenu { get; set; }
    public List<Route>? CollapsedRoutes { get; set; }

    public RouteMenuSettings Main(string icon, string displayName)
    {
        IsMain = true;
        DisplayName = displayName;
        Icon = icon;

        return this;
    }

    public RouteMenuSettings Secondary(string icon, string displayName)
    {
        IsMain = false;
        IsHidden = false;
        DisplayName = displayName;
        Icon = icon;

        return this;
    }

    public RouteMenuSettings HasStyleClass(IList<string>? styleClass)
    {
        StyleClass = styleClass;

        return this;
    }
}
