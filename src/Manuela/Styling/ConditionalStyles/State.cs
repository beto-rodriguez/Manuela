// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class State : ConditionalStyle
{
    public State()
    {
        Condition = new(visualElement => Name == (string?)visualElement.GetValue(Has.CustomStateProperty))
        {
            Triggers = v => [new(v, [Has.CustomStateProperty.PropertyName])]
        };
    }

    public string? Name { get; set; }
}
