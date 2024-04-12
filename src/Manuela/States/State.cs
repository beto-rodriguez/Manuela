// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States;

namespace Manuela;

public class State : ConditionalStyle
{
    public State()
    {
        Condition = new(visualElement => Name == (string?)visualElement.GetValue(Has.CustomStateProperty));
    }

    public string? Name { get; set; }

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(visualElement, [Has.CustomStateProperty.PropertyName])];
    }
}
