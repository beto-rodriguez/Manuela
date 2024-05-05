using Manuela.States.Screen;
using Manuela.Things;

namespace Manuela.Styling;

public static class StylingExtensions
{
    public static string GetCustomState(this VisualElement visualElement)
    {
        return (string)visualElement.GetValue(Has.CustomStateProperty);
    }

    public static void SetCustomState(this VisualElement visualElement, string? state)
    {
        visualElement.SetValue(Has.CustomStateProperty, state);
    }

    public static Breakpoint GetScreenBreakpoint(this VisualElement visualElement)
    {
        return OnScreenSize.GetBreakpoint(visualElement, visualElement.Window);
    }

    public static void SetManuelaStyle(this VisualElement visualElement, object setters, bool animated = true)
    {
        SetManuelaStyle(visualElement, (ManuelaSettersDictionary)setters);
    }

    public static void SetManuelaStyle(this VisualElement visualElement, ManuelaSettersDictionary setters, bool animated = true)
    {
        foreach (var setter in setters)
            SetManuelaProperty(visualElement, setter.Key, setter.Value, animated);
    }

    public static void SetManuelaProperty(this VisualElement visualElement, ManuelaProperty property, object? value, bool animated = true)
    {
        var transitions = (TransitionsCollection?)visualElement.GetValue(Has.TransitionsProperty);

        var bindableProperty = ManuelaThings.GetBindableProperty(visualElement, property)
            ?? throw new NotImplementedException(
                $"Manuela was not able to resolve the {property} property o the {visualElement.GetType()} type.");

        if (
            animated &&
            value is not null &&
            transitions is not null &&
            transitions.TryGetValue(visualElement, property, out var transition, out var isFirst)
            // &&
            //!isFirst
            )
        {
            // animate if a transition is defined
            // also ignore nulls... how do we animate nulls?
            // .. also skip first time, as we are setting the value for the first time.
            var animation = ManuelaThings.GetAnimation(visualElement, bindableProperty, value);
            animation.Commit(visualElement, $"{property} animation", easing: transition.Easing, length: transition.Duration);
        }
        else
        {
            visualElement.SetValue(bindableProperty, value);
        }
    }
}
