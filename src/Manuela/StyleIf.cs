using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace Manuela;

public class StyleIf : Element
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ConditionProperty = BindableProperty.Create(
        nameof(Condition), typeof(Condition), typeof(StatesCollection), null);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public Style Style { get; set; }

    public Condition? Condition
    {
        get => (Condition)GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    // Note #1
    // initialization must be per bindable.
    // to avoid a possible issue when a resource is shared using x:StaticResource.
    public HashSet<VisualElement> IsInitialized { get; } = [];

    public void Initialize(VisualElement visual)
    {
        var triggers = Condition?.Triggers(visual) ?? [];

        foreach (var trigger in triggers)
        {
            trigger.Target.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName is null || !trigger.Properties.Contains(e.PropertyName))
                    return;

                // at this point, the target property changed and the condition was met
                Apply(visual);
            };
        }

        _ = IsInitialized.Add(visual);
        Apply(visual);
    }

    public void Apply(VisualElement? visual)
    {
        if (visual is null || !IsInitialized.Contains(visual)) return;

        var responsiveStyle = (ResponsiveStyle?)visual.GetValue(On.ResponsiveStyleProperty);

        foreach (var property in Style.Setters.Keys)
        {
            var bindableProperty = ManuelaThings.GetBindableProperty(visual, property);

            if (bindableProperty is null)
            {
#if DEBUG
                Trace.WriteLine($"Property {property} is not supported on {visual.GetType().Name}");
#endif
                continue;
            }

            var conditionMet = ApplyPropertyIfMet(visual, property, bindableProperty);

            if (!conditionMet)
            {
                if (responsiveStyle is null)
                    visual.ClearValue(bindableProperty);
                else
                    responsiveStyle.ApplyProperty(visual, property, bindableProperty);
            }
        }
    }

    public bool ApplyPropertyIfMet(VisualElement visual, ManuelaProperty property, BindableProperty bindableProperty)
    {
        if (!Style.Setters.TryGetValue(property, out var value)) return false;
        if (!Condition?.Predicate(visual) ?? false) return false;

        value = ManuelaThings.TryConvert(visual, property, value);
        visual.SetValue(bindableProperty, value);

        return true;
    }
}
