// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;

namespace Manuela;

public class Has
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty StylesProperty = BindableProperty.CreateAttached(
        "Styles", typeof(StylesCollection), typeof(Has), null, propertyChanged: OnStyleCollectionChanged);

    public static BindableProperty TransitionsProperty = BindableProperty.CreateAttached(
        "Transitions", typeof(TransitionsCollection), typeof(Has), null, propertyChanged: OnTransitionsCollectionChanged);

    public static BindableProperty DefaultStyleProperty = BindableProperty.CreateAttached(
        "DefaultStyle", typeof(ManuelaSettersDictionary), typeof(Has), null, propertyChanged: OnDefaultStyleChanged);

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

    public static StylesCollection GetStyles(BindableObject view) => (StylesCollection)view.GetValue(StylesProperty);
    public static void SetStyles(BindableObject view, StylesCollection value) => view.SetValue(StylesProperty, value);

    public static ManuelaSettersDictionary GetDefaultStyle(BindableObject view) => (ManuelaSettersDictionary)view.GetValue(StylesProperty);
    public static void SetDefaultStyle(BindableObject view, ManuelaSettersDictionary value) => view.SetValue(DefaultStyleProperty, value);

    public static TransitionsCollection GetTransitions(BindableObject view) => (TransitionsCollection)view.GetValue(TransitionsProperty);
    public static void SetTransitions(BindableObject view, TransitionsCollection value) => view.SetValue(TransitionsProperty, value);

    public static void OnStyleCollectionChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not VisualElement ve) return;

        if (oldValue is not null and StylesCollection oldStylesCollection)
        {
            oldStylesCollection.Dispose();
            foreach (var oldStyle in oldStylesCollection) oldStyle.Dispose();
        }

        var styleCollection = (StylesCollection?)newValue ?? [];

        // because the default style is internal and not part of the collection
        // we add it here
        var defaultStyle = ve.GetValue(DefaultStyleProperty) as ManuelaSettersDictionary;
        styleCollection.Add(new Default { Setters = defaultStyle });

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

    public static void OnDefaultStyleChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        var newSetters = (ManuelaSettersDictionary?)newValue;
        var stylesCollection = (StylesCollection?)bindable.GetValue(StylesProperty);

        if (stylesCollection is null)
        {
            bindable.SetValue(StylesProperty, new StylesCollection { new Default { Setters = newSetters } });
            return;
        }

        var defaultStyle = stylesCollection.FirstOrDefault(x => x is Default);

        if (defaultStyle is not null)
        {
            if (newSetters is null)
            {
                _ = stylesCollection.Remove(defaultStyle);
                return;
            }

            defaultStyle.Setters = newSetters;
            return;
        }

        stylesCollection.Add(new Default { Setters = newSetters });
    }

    public static void OnTransitionsCollectionChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (oldValue is not null and TransitionsCollection oldCollection)
        {
            oldCollection.Dispose();
        }
    }
}
