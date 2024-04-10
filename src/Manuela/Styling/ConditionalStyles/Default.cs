// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class Default : ConditionalStyle
{
    public Default()
    {
        Condition = new(visualElement => true);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(visualElement, [])];
    }
}
