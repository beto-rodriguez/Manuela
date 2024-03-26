namespace Manuela.Styling.ConditionalStyles;

public class Disabled : ConditionalStyle
{
    public Disabled()
    {
        Condition = new(visualElement => !visualElement.IsEnabled)
        {
            Triggers = v => [new(v, [nameof(VisualElement.IsEnabled)])]
        };
    }
}
