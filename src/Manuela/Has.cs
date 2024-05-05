// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States.Screen;

namespace Manuela;

public class Has
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty StatesProperty = BindableProperty.CreateAttached(
        "States", typeof(StatesCollection), typeof(Has), null, propertyChanged: OnStyleCollectionChanged);

    public static BindableProperty TransitionsProperty = BindableProperty.CreateAttached(
        "Transitions", typeof(TransitionsCollection), typeof(Has), null, propertyChanged: OnTransitionsCollectionChanged);

    public static BindableProperty ScreenBreakPointProperty = BindableProperty.CreateAttached(
        "ScreenBreakPoint", typeof(Breakpoint), typeof(Has), Breakpoint.Xs);

    public static BindableProperty IsHoverStateProperty = BindableProperty.CreateAttached(
        "IsHoverState", typeof(bool), typeof(Has), false);

    public static BindableProperty IsPressedStateProperty = BindableProperty.CreateAttached(
        "IsPressedState", typeof(bool), typeof(Has), false);

    public static BindableProperty IsCheckedStateProperty = BindableProperty.CreateAttached(
        "IsCheckedState", typeof(bool), typeof(Has), false);

    public static BindableProperty CustomStateProperty = BindableProperty.CreateAttached(
        "CustomState", typeof(string), typeof(Has), null);

    public static BindableProperty ModalTcsProperty = BindableProperty.CreateAttached(
        "ModalTcs", typeof(TaskCompletionSource<object>), typeof(Has), null);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static StatesCollection GetStates(BindableObject view) => (StatesCollection)view.GetValue(StatesProperty);
    public static void SetStates(BindableObject view, StatesCollection value) => view.SetValue(StatesProperty, value);

    public static TransitionsCollection GetTransitions(BindableObject view) => (TransitionsCollection)view.GetValue(TransitionsProperty);
    public static void SetTransitions(BindableObject view, TransitionsCollection value) => view.SetValue(TransitionsProperty, value);

    public static TaskCompletionSource<object> GetModalTcs(BindableObject view) => (TaskCompletionSource<object>)view.GetValue(ModalTcsProperty);
    public static void SetModalTcs(BindableObject view, TaskCompletionSource<object> value) => view.SetValue(ModalTcsProperty, value);

    public static void OnStyleCollectionChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not VisualElement ve) return;

        if (oldValue is not null and StatesCollection oldStylesCollection)
        {
            foreach (var oldStyle in oldStylesCollection)
                foreach (var visual in oldStyle.InitializedElements)
                    oldStyle.Dispose(visual);

            oldStylesCollection.Dispose();
        }

        var styleCollection = (StatesCollection?)newValue ?? [];
        styleCollection.Initialize(ve);

        foreach (var style in styleCollection)
        {
            if (ve.IsLoaded) style.Initialize(ve);
            else ve.Loaded += (_, _) => style.Initialize(ve);

            ve.Unloaded += (_, _) =>
            {
                style.Dispose(ve);
                if (style.InitializedElements.Count == 0)
                    styleCollection.Dispose();
            };
        }
    }

    public static void OnTransitionsCollectionChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (oldValue is not null and TransitionsCollection oldCollection)
        {
            oldCollection.Dispose();
        }
    }
}
