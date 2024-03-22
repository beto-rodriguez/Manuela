using System.Diagnostics;
using Manuela.Expressions;

namespace Manuela;

[ContentProperty(nameof(Style))]
public class StyleIf : Element
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ConditionProperty = BindableProperty.Create(
        nameof(Condition), typeof(XamlCondition), typeof(StatesCollection), null);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public Style Style { get; set; }

    public XamlCondition? Condition
    {
        get => (XamlCondition)GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    // Note #1
    // initialization must be per bindable.
    // to avoid a possible issue when a resource is shared using x:StaticResource.
    public HashSet<VisualElement> IsInitialized { get; } = [];

    public void Initialize(VisualElement visual)
    {
        if (Condition?.Triggers is null)
            throw new Exception(
                "Manuela was not able to find the Expression triggers. " +
                "Ensure the InitializeTriggers() method is called.");

        var triggers = Condition?.Triggers(visual) ?? [];

        foreach (var trigger in triggers)
        {
            trigger.Notifier.PropertyChanged += (sender, e) =>
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

public class FocusedState : StyleIf
{
    public FocusedState()
    {
        Condition = new XamlCondition(visualElement => visualElement.IsFocused)
        {
            Triggers = v => [new(v, [nameof(VisualElement.IsFocused)])]
        };
    }
}


public class DisabledState : StyleIf
{
    public DisabledState()
    {
        Condition = new XamlCondition(visualElement => !visualElement.IsEnabled)
        {
            Triggers = v => [new(v, [nameof(VisualElement.IsEnabled)])]
        };
    }
}
