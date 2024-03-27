using Manuela.Styling.ConditionalStyles;

namespace Manuela.Styling;

public class StylesCollection : List<ConditionalStyle>
{
    public bool ApplyPropertyIfMet(
        VisualElement visual,
        ManuelaProperty property,
        BindableProperty bindableProperty,
        TransitionsCollection? transitions)
    {
        // as soon as one condition is met, we can stop
        // we should be applying on the same property multiple times.

        foreach (var conditionalStyle in this)
        {
            var conditionMet = conditionalStyle.ApplyPropertyIfMet(visual, property, bindableProperty, transitions);
            if (conditionMet) return true;
        }

        return false;
    }
}
