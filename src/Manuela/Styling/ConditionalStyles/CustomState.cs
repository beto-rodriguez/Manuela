// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class CustomState : ConditionalStyle
{
    public CustomState()
    {
        Condition = new(visualElement => State == (string?)visualElement.GetValue(Has.CustomStateProperty))
        {
            Triggers = v => [new(v, [Has.CustomStateProperty.PropertyName])]
        };
    }

    public string? State { get; set; }
}
