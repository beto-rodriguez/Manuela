using System.Diagnostics;
using Manuela.Expressions;

namespace Manuela;

[ContentProperty(nameof(Style))]
public class ConditionalStyle : Element
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ConditionProperty = BindableProperty.Create(
        nameof(Condition), typeof(XamlCondition), typeof(StatesCollection), null);
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
        {
            trigger.Notifier.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName is null || !trigger.Properties.Contains(e.PropertyName))
                    return;

                // at this point we know that a property that was declared as a trigger has changed.
                Apply(visual);
            };
        }

        _ = IsInitialized.Add(visual);
        Apply(visual);
    }

    public void Apply(VisualElement? visual)
    {
        if (visual is null || !IsInitialized.Contains(visual)) return;

        var keys = Style?.Setters.Keys;
        if (keys is null) return;

        var allStates = (StatesCollection?)visual.GetValue(Has.StatesProperty);

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

public class LargeScreen : ConditionalStyle
{
    public LargeScreen()
    {
        Condition = new XamlCondition(visualElement => visualElement.Window?.Width > 1024)
        {
            Triggers = v => [new(v.Window, [nameof(v.Window.Width)])]
        };
    }
}

public class Focused : ConditionalStyle
{
    public Focused()
    {
        Condition = new XamlCondition(visualElement => visualElement.IsFocused)
        {
            Triggers = v => [new(v, [nameof(VisualElement.IsFocused)])]
        };
    }
}

public class Disabled : ConditionalStyle
{
    public Disabled()
    {
        Condition = new XamlCondition(visualElement => !visualElement.IsEnabled)
        {
            Triggers = v => [new(v, [nameof(VisualElement.IsEnabled)])]
        };
    }
}

public class Hovered : ConditionalStyle
{
    public Hovered()
    {
        Condition = new XamlCondition(visualElement => (bool)visualElement.GetValue(Has.IsHoveredProperty))
        {
            Triggers = v =>
            {
#if DEBUG
                if (v is not View view)
                    throw new Exception(
                        $"{nameof(Hovered)} trigger is not supported in elements of type {v.GetType()}. " +
                        $"The type does not inherit from {nameof(View)}");
#endif

                var pointerRecognizer = new PointerGestureRecognizer();

                pointerRecognizer.PointerEntered += (sender, e) =>
                {
                    if (sender is null || sender is not BindableObject bindable) return;
                    bindable.SetValue(Has.IsHoveredProperty, true);
                };

                pointerRecognizer.PointerExited += (sender, e) =>
                {
                    if (sender is null || sender is not BindableObject bindable) return;
                    bindable.SetValue(Has.IsHoveredProperty, false);
                };

                view.GestureRecognizers.Add(pointerRecognizer);

                return [new(v, ["IsHovered"])];
            }
        };
    }
}

public class Selected : ConditionalStyle
{
    public Selected()
    {
        Condition = new XamlCondition(visualElement => false)
        {
            Triggers = v =>
            {
                var selectedState = new VisualState { Name = "Selected" };
                foreach (var item in Style?.AsSetters(v) ?? [])
                    selectedState.Setters.Add(item);

                v.SetValue(
                    VisualStateManager.VisualStateGroupsProperty,
                    new VisualStateGroupList
                    {
                        new VisualStateGroup
                        {
                            Name = "CommonStates",
                            States =
                            {
                                new VisualState { Name = "Normal" },
                                selectedState
                            }
                        }
                    });

                return [new(v, [])];
            }
        };
    }
}

