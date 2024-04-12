// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

namespace Manuela;

public class Unchecked : Checked
{
    protected override bool ConditionDefinition(VisualElement visualElement)
    {
        return !base.ConditionDefinition(visualElement);
    }
}
