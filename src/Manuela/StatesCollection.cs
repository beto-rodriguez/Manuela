namespace Manuela;

public class StatesCollection : List<ConditionalStyle>
{
    public bool ApplyPropertyIfMet(VisualElement visual, ManuelaProperty property, BindableProperty bindableProperty)
    {
        // as soon as one condition is met, we can stop
        // we should be applying on the same property multiple times.

        foreach (var conditionalStyle in this)
        {
            var conditionMet = conditionalStyle.ApplyPropertyIfMet(visual, property, bindableProperty);
            if (conditionMet) return true;
        }

        return false;
    }
}
