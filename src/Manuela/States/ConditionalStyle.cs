using System.Diagnostics;
using Manuela.Expressions;
using Manuela.Styling;
using Manuela.Things;

namespace Manuela.States;

[ContentProperty(nameof(Setters))]
public class ConditionalStyle
{
    private ManuelaSettersDictionary? _setters;
    private XamlCondition? _condition;

    public ManuelaSettersDictionary? Setters
    {
        set
        {
            if (_setters is not null)
                foreach (var visualElement in InitializedElements)
                    ClearValues(visualElement, _setters.Keys);

            _setters = value; ReApply();
        }
    }

    protected internal XamlCondition Condition
    {
        get => _condition ?? XamlCondition.Empty; // "allow" nulls, a hack for hot reload.
        set { _condition = value; ReApply(); }
    }

    // initialization must be per visual.
    // to avoid a possible issue when the state is shared between mupltiple elements.
    public HashSet<VisualElement> InitializedElements { get; } = [];

    public void Initialize(VisualElement visual, StatesCollection collection)
    {
        if (InitializedElements.Contains(visual)) return;

        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += OnThemeChanged;

        OnInitialized(visual);

        // a "trigger" is an object that has at least 2 things:
        //   1. An INotifyPropertyChanged object.
        //   2. A HashSet containing the property names in the INPC object that fire an update
        // finally we attach a handler to each INPC and listen for changes in the target properties.

        foreach (var trigger in Condition.Triggers)
        {
            // null notifiers are valid. the user has the freedom to pass a null intance.
            if (trigger.Notifier is null) continue;

            // save a reference to the handler, this handler has a capture on the "visual" reference.
            // this way we should be able to unsubscribe from the PropertyChanged event when the state is disposed.

            trigger.NotifierHandler = (sender, e) =>
            {
                if (e.PropertyName is null || !trigger.Properties.Contains(e.PropertyName))
                    return;

                // at this point we know that the property that was declared as a trigger has changed.
                Apply(visual);
            };

            trigger.Notifier.PropertyChanged += trigger.NotifierHandler;
        }

        _ = InitializedElements.Add(visual);
        Apply(visual);

        if (!Router.Current.ActiveRoute.IsSingleton)
        {
            visual.Unloaded += (_, _) =>
            {
                Dispose(visual);

                if (InitializedElements.Count == 0)
                    collection.Dispose();
            };
        }
    }

    public virtual ManuelaSettersDictionary? GetSetters()
    {
        return _setters;
    }

    public void Apply(VisualElement? applyTarget)
    {
        if (applyTarget is null || !InitializedElements.Contains(applyTarget)) return;

        var keys = GetSetters()?.Keys;
        if (keys is null) return;

        var allStyles = (StatesCollection?)applyTarget.GetValue(Has.StatesProperty);
        var transitions = (TransitionsCollection?)applyTarget.GetValue(Has.TransitionsProperty);

        foreach (var property in keys)
        {
            var bindableProperty = ManuelaThings.GetBindableProperty(applyTarget, property);

            if (bindableProperty is null)
            {
#if DEBUG
                Trace.WriteLine($"Property {property} is not supported on {applyTarget.GetType().Name}");
#endif
                continue;
            }

            var conditionMet = ApplyPropertyIfMet(applyTarget, property, bindableProperty, transitions);

            if (!conditionMet)
            {
                var anyOtherStateMet = allStyles?.ApplyPropertyIfMet(applyTarget, property, bindableProperty, transitions)
                    ?? false;

                if (!anyOtherStateMet)
                    applyTarget.ClearValue(bindableProperty);
            }
        }
    }

    public bool ApplyPropertyIfMet(
        VisualElement applyTarget,
        ManuelaProperty property,
        BindableProperty bindableProperty,
        TransitionsCollection? transitions)
    {
        var setters = GetSetters();

        if (setters is null || !setters.TryGetValue(property, out var value)) return false;
        if (!Condition?.Predicate(applyTarget) ?? false) return false;

        value = ManuelaThings.TryConvert(applyTarget, property, value);

        if (                                                                                    // do not animate if:
            value is not null &&                                                                // target value is null
            transitions is not null &&                                                          // there are no transitions
            transitions.TryGetValue(applyTarget, property, out var transition, out var isFirst)      // the property does not have a transition
            // && !isFirst    <- is this neccesary?                                             // is the first time the property is being set
            )
        {
            var animation = ManuelaThings.GetAnimation(applyTarget, bindableProperty, value);
            animation.Commit(applyTarget, $"{property} animation", easing: transition.Easing, length: transition.Duration);
        }
        else
        {
            applyTarget.SetValue(bindableProperty, value);
        }

        return true;
    }

    public void ClearValues(VisualElement visualElement, Dictionary<ManuelaProperty, object?>.KeyCollection? keys = null)
    {
        keys ??= GetSetters()?.Keys;
        if (keys is null) return;

        foreach (var key in keys)
        {
            var bindableProperty = ManuelaThings.GetBindableProperty(visualElement, key);
            if (bindableProperty is null) continue;

            visualElement.SetValue(bindableProperty, bindableProperty.DefaultValue);
        }
    }

    public virtual void Dispose(VisualElement visualElement)
    {
        ClearValues(visualElement);
        _ = InitializedElements.Remove(visualElement);

        if (InitializedElements.Count > 0) return;

        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged -= OnThemeChanged;

        foreach (var trigger in Condition.Triggers)
        {
            if (trigger.Notifier is null) continue;
            trigger.Notifier.PropertyChanged -= trigger.NotifierHandler;
        }

        Condition = null!;
    }

    protected void ReApply()
    {
        foreach (var visualElement in InitializedElements)
            Apply(visualElement); // this is just used in hot reload.
    }

    protected virtual void OnInitialized(VisualElement visualElement) { }

    private void OnThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        ReApply();
    }
}
