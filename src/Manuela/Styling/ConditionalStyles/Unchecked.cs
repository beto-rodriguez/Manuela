namespace Manuela.Styling.ConditionalStyles;

public class Unchecked : Checked
{
    protected override bool ConditionDefinition(VisualElement visualElement)
    {
        return !base.ConditionDefinition(visualElement);
    }
}
