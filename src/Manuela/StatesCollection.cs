namespace Manuela;

public class StatesCollection : List<StyleIf>
{
    public bool ApplyFirstPropertyMet(VisualElement visual, ManuelaProperty property, BindableProperty bindableProperty)
    {
        foreach (var state in this)
            if (state.ApplyPropertyIfMet(visual, property, bindableProperty)) return true;

        return false;
    }
}
