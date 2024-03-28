﻿// Note #ABOUT-XAML-NS
// should we create a custom ns schema for this?
// https://learn.microsoft.com/en-us/dotnet/maui/xaml/namespaces/custom-namespace-schemas?view=net-maui-8.0
// the problem is:
// https://learn.microsoft.com/en-us/dotnet/maui/xaml/namespaces/custom-namespace-schemas?view=net-maui-8.0#consume-a-custom-namespace-schema
// is that really necessary?

// as a workaround we do it the old way... expose everything in this namespace

namespace Manuela;

public class AppPage : Controls.AppPage { }
public class AppBody : Controls.AppBody { }

public class SetExtension : Styling.SetExtension { }
public class StylesCollection : Styling.StylesCollection
{
    public StylesCollection()
    { }

    public StylesCollection(List<Styling.ConditionalStyles.ConditionalStyle> list) : base(list) { }
}

public class TransitionsCollection : Transitions.TransitionsCollection { }
public class Transition : Transitions.Transition { }

public class Checked : Styling.ConditionalStyles.Checked { }
public class Disabled : Styling.ConditionalStyles.Disabled { }
public class Focused : Styling.ConditionalStyles.Focused { }
public class Hovered : Styling.ConditionalStyles.Hovered { }
public class Normal : Styling.ConditionalStyles.Normal { }
public class Pressed : Styling.ConditionalStyles.Pressed { }
public class Selected : Styling.ConditionalStyles.Selected { }
public class Unchecked : Styling.ConditionalStyles.Unchecked { }

public class OnXsScreen : Styling.ConditionalStyles.Screen.OnXsScreen { }
public class OnSmallScreen : Styling.ConditionalStyles.Screen.OnSmallScreen { }
public class OnMediumScreen : Styling.ConditionalStyles.Screen.OnMediumScreen { }
public class OnLargeScreen : Styling.ConditionalStyles.Screen.OnLargeScreen { }
public class OnXlScreen : Styling.ConditionalStyles.Screen.OnXlScreen { }
public class OnXxlScreen : Styling.ConditionalStyles.Screen.OnXxlScreen { }

public class OnDesktop : Styling.ConditionalStyles.Device.OnDesktop { }
public class OnPhone : Styling.ConditionalStyles.Device.OnPhone { }
public class OnTablet : Styling.ConditionalStyles.Device.OnTablet { }
public class OnTV : Styling.ConditionalStyles.Device.OnTV { }
public class OnWatch : Styling.ConditionalStyles.Device.OnWatch { }

public class OnAndroid : Styling.ConditionalStyles.Platform.OnPlatformAndroid { }
public class OnIOS : Styling.ConditionalStyles.Platform.OnPlatformIOS { }
public class OnMacOS : Styling.ConditionalStyles.Platform.OnPlatformMacOS { }
public class OnTizen : Styling.ConditionalStyles.Platform.OnPlatformTizen { }
public class OnWatchOS : Styling.ConditionalStyles.Platform.OnPlatformWatchOS { }
public class OnWindows : Styling.ConditionalStyles.Platform.OnPlatformWindows { }

// it seems that Xaml intellisense does not work if attached properties are inherited..
// lets write it here...
public class Has
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty StylesProperty = BindableProperty.CreateAttached(
        "Styles", typeof(StylesCollection), typeof(Has), null, propertyChanged: OnStyleCollectionChanged);

    public static BindableProperty ScreenBreakPointProperty = BindableProperty.CreateAttached(
        "ScreenBreakPoint", typeof(Styling.ConditionalStyles.Screen.Breakpoint), typeof(Has), Styling.ConditionalStyles.Screen.Breakpoint.Xs);

    public static BindableProperty IsHoverStateProperty = BindableProperty.CreateAttached(
        "IsHoverState", typeof(bool), typeof(Has), false);

    public static BindableProperty IsPressedStateProperty = BindableProperty.CreateAttached(
        "IsPressedState", typeof(bool), typeof(Has), false);

    public static BindableProperty IsCheckedStateProperty = BindableProperty.CreateAttached(
        "IsCheckedState", typeof(bool), typeof(Has), false);

    public static BindableProperty IsValidStateProperty = BindableProperty.CreateAttached(
        "IsValidState", typeof(bool), typeof(Has), true);

    public static BindableProperty TransitionsProperty = BindableProperty.CreateAttached(
        "Transitions", typeof(TransitionsCollection), typeof(Has), null);
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
}

// it seems that Xaml intellisense does not work if attached properties are inherited..
// lets write it here...
public class Router
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty LinkProperty = BindableProperty.CreateAttached(
        "Link", typeof(string), typeof(Has), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var route = (string)newValue;

            if (bindable is Button button)
            {
                button.Clicked += (_, _) => AppRouting.Routing.GoTo(route);

                return;
            }

            if (bindable is not View view) return;

            view.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => AppRouting.Routing.GoTo(route))
            });
        });
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static string GetLink(BindableObject view) => (string)view.GetValue(LinkProperty);
    public static void SetLink(BindableObject view, string value) => view.SetValue(LinkProperty, value);
}
