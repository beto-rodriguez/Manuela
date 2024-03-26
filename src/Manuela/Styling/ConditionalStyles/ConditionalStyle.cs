using System.Diagnostics;
using Manuela.Expressions;

namespace Manuela.Styling.ConditionalStyles;

[ContentProperty(nameof(Style))]
public class ConditionalStyle : Element
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ConditionProperty = BindableProperty.Create(
        nameof(Condition), typeof(XamlCondition), typeof(StylesCollection), null);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public Style? Style { get; set; }

    public XamlCondition? Condition
    {
        get => (XamlCondition)GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    // initialization must be per visual.
    // to avoid a possible issue when a resource is shared using x:StaticResource.
    public HashSet<VisualElement> IsInitialized { get; } = [];

    public void Initialize(VisualElement visual)
    {
        if (IsInitialized.Contains(visual)) return;

        if (Condition?.Triggers is null)
            throw new Exception(
                "Manuela was not able to find the Expression triggers. " +
                "Ensure the InitializeTriggers() method is called.");

        foreach (var trigger in Condition?.Triggers(visual) ?? [])
            trigger.Notifier.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName is null || !trigger.Properties.Contains(e.PropertyName))
                    return;

                // at this point we know that a property that was declared as a trigger has changed.
                Apply(visual);
            };

        _ = IsInitialized.Add(visual);
        Apply(visual);
    }

    public void Apply(VisualElement? visual)
    {
        if (visual is null || !IsInitialized.Contains(visual)) return;

        var keys = Style?.Setters.Keys;
        if (keys is null) return;

        var allStates = (StylesCollection?)visual.GetValue(Has.StylesProperty);

        foreach (var property in keys)
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
                var anyOtherStateMet = allStates?.ApplyPropertyIfMet(visual, property, bindableProperty)
                    ?? false;

                if (!anyOtherStateMet)
                    visual.ClearValue(bindableProperty);
            }
        }
    }

    public bool ApplyPropertyIfMet(VisualElement visual, ManuelaProperty property, BindableProperty bindableProperty)
    {
        if (Style is null || !Style.Setters.TryGetValue(property, out var value)) return false;
        if (!Condition?.Predicate(visual) ?? false) return false;

        value = ManuelaThings.TryConvert(visual, property, value);
        visual.SetValue(bindableProperty, value);

#if DEBUG
        Trace.WriteLine($"Applied {property} to {value}");
#endif

        return true;
    }
}

