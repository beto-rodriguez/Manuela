// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States;

namespace Manuela;

public class OnRoute : ConditionalStyle
{
    private static readonly Dictionary<string, HashSet<string>> s_alias = [];

    public OnRoute()
    {
        Condition = new(visualElement => GetNameOrAlias(Router.Current.ActiveRoute.RouteName) == Name);
    }

    public string? Name { get; set; } = "{empty}";

    public static HashSet<string> Transitions { get; set; } = [];

    public static void AddAlias(string alias, params string[] routes)
    {
        if (!s_alias.TryGetValue(alias, out var aliasCollection))
            s_alias[alias] = aliasCollection = [];

        foreach (var route in routes)
            _ = aliasCollection.Add(route);
    }

    public static string GetNameOrAlias(string route)
    {
        foreach (var (alias, routes) in s_alias)
        {
            if (routes.Contains(route))
                return alias;
        }

        return route;
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(Router.Current, [nameof(Router.ActiveRoute)])];
    }
}
