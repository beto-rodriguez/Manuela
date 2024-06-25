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
        List<string>? styleClass = null,
        bool isSingleton = false)
    {
        _routes.Add(new Route(viewType, viewModelType, routeName, s =>
        {
            s.DisplayName = displayName;
            s.Icon = icon;
            s.StyleClass = styleClass;
        }, isSingleton));

        return this;
    }

    public CollapsedBuilder Add<TView>(
        string routeName,
        string displayName,
        string icon,
        AppView<TView> viewSettings,
        List<string>? styleClass = null,
        bool isSingleton = false)
    {
        return Add(
            viewSettings.ViewType, null, routeName, displayName, icon, styleClass, isSingleton);
    }

    public CollapsedBuilder Add<TView, TViewModel>(
        string routeName,
        string displayName,
        string icon,
        AppView<TView, TViewModel> viewSettings,
        List<string>? styleClass = null,
        bool isSingleton = false)
    {
        return Add(
            viewSettings.ViewType, viewSettings.ViewModelType, routeName, displayName, icon, styleClass, isSingleton);
    }
}
