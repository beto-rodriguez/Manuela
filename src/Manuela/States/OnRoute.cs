// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States;

namespace Manuela;

public class OnRoute : ConditionalStyle
{
    public OnRoute()
    {
        Condition = new(visualElement => Router.Current.ActiveRoute.RouteName == Name);
    }

    public string? Name { get; set; } = "{empty}";

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(Router.Current, [nameof(Router.ActiveRoute)])];
    }

}
