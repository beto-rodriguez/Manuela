// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles.Screen;

namespace Manuela;

// it seems that Xaml intellisense does not work if attached properties are inherited..
// lets write it here...
public class Has
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty StylesProperty = BindableProperty.CreateAttached(
        "Styles", typeof(StylesCollection), typeof(Has), null, propertyChanged: OnStyleCollectionChanged);

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

    public static BindableProperty IsValidStateProperty = BindableProperty.CreateAttached(
        "IsValidState", typeof(bool), typeof(Has), true);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static StylesCollection GetStyles(BindableObject view)
    {
        return (StylesCollection)view.GetValue(StylesProperty);
    }

    public static void SetStyles(BindableObject view, StylesCollection value)
    {
        view.SetValue(StylesProperty, value);
    }

    public static TransitionsCollection GetTransitions(BindableObject view)
    {
        return (TransitionsCollection)view.GetValue(TransitionsProperty);
    }

    public static void SetTransitions(BindableObject view, TransitionsCollection value)
    {
        view.SetValue(TransitionsProperty, value);
    }

    public static void OnStyleCollectionChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not VisualElement ve) return;

        if (oldValue is not null and StylesCollection oldStylesCollection)
        {
            oldStylesCollection.Dispose();
            foreach (var oldStyle in oldStylesCollection) oldStyle.Dispose();
        }

        var styleCollection = (StylesCollection?)newValue ?? [];
        styleCollection.Initialize(ve);

        foreach (var style in styleCollection)
        {
            if (ve.IsLoaded) style.Initialize(ve);
            else ve.Loaded += (_, _) => style.Initialize(ve);

            // on data templates it seems that the loaded event is not fired...
            // possible workaround is to use SizeChanged:
            // ve.SizeChanged += (_, _) => style.Initialize(ve);
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
