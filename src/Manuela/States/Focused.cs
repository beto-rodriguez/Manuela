using Manuela.States;

namespace Manuela;

public class Focused : ConditionalStyle
{
    public Focused()
    {
        Condition = new(visualElement => visualElement.IsFocused);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(visualElement, [nameof(VisualElement.IsFocused)])];
    }
}
