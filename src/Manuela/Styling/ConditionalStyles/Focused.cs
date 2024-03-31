using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class Focused : ConditionalStyle
{
    public Focused()
    {
        Condition = new(visualElement => visualElement.IsFocused)
        {
            Triggers = v => [new(v, [nameof(VisualElement.IsFocused)])]
        };
    }
}
