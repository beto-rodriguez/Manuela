namespace Manuela;

public class Has
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty StatesProperty = BindableProperty.CreateAttached(
        "States", typeof(StatesCollection), typeof(Has), null, propertyChanged: OnStateChanged);

    public static BindableProperty IsHoveredProperty = BindableProperty.CreateAttached(
        "IsHovered", typeof(bool), typeof(Has), false);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static StatesCollection GetStates(BindableObject view)
    {
        return (StatesCollection)view.GetValue(StatesProperty);
    }

    public static void SetStates(BindableObject view, StatesCollection value)
    {
        view.SetValue(StatesProperty, value);
    }

    public static void OnStateChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (newValue is null || bindable is not VisualElement ve) return;

        var statesCollection = (StatesCollection?)newValue ?? [];

        foreach (var conditionalSet in statesCollection)
        {
            ve.AddLogicalChild(conditionalSet);

            if (ve.IsLoaded)
            {
                conditionalSet.Initialize(ve);
            }
            else
            {
                ve.Loaded += (_, _) =>
                    conditionalSet.Initialize(ve);
            }

            ve.SizeChanged += (_, _) =>
                conditionalSet.Initialize(ve);
        }
    }
}
