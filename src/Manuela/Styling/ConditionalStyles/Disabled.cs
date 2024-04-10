// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class Disabled : ConditionalStyle
{
    public Disabled()
    {
        Condition = new(visualElement => !visualElement.IsEnabled);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(visualElement, [nameof(VisualElement.IsEnabled)])];
    }
}
