// should we create a custom ns schema for this?
// https://learn.microsoft.com/en-us/dotnet/maui/xaml/namespaces/custom-namespace-schemas?view=net-maui-8.0
// the problem is:
// https://learn.microsoft.com/en-us/dotnet/maui/xaml/namespaces/custom-namespace-schemas?view=net-maui-8.0#consume-a-custom-namespace-schema
// is that really necessary?

// as a workaround we do it the old way... expose everything in this ns.

namespace Manuela;

public class AppPage : Controls.AppPage { }

public class AppBody : Controls.AppBody { }

// it seems that Xaml intellisense does not work with this approach..
// lets write it here...
public class Has
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty StylesProperty = BindableProperty.CreateAttached(
        "Styles", typeof(StylesCollection), typeof(Has), null, propertyChanged: OnStyleCollectionChanged);

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

    public static void OnStyleCollectionChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (newValue is null || bindable is not VisualElement ve) return;

        var styleCollection = (StylesCollection?)newValue ?? [];

        foreach (var style in styleCollection)
        {
            ve.AddLogicalChild(style);

            if (ve.IsLoaded) style.Initialize(ve);
            else ve.Loaded += (_, _) => style.Initialize(ve);

            // on data templates it seems that the loaded event is not fired...
            // possible workaround is to use SizeChanged:
            // ve.SizeChanged += (_, _) => style.Initialize(ve);
        }
    }
}

public class StyleExtension : Styling.StyleExtension { }

public class StylesCollection : Styling.StylesCollection { }

public class Checked : Styling.ConditionalStyles.Checked { }

public class Disabled : Styling.ConditionalStyles.Disabled { }

public class Focused : Styling.ConditionalStyles.Focused { }

public class Hovered : Styling.ConditionalStyles.Hovered { }

public class Normal : Styling.ConditionalStyles.Normal { }

public class OnScreenLarge : Styling.ConditionalStyles.OnScreenLarge { }

public class OnScreenMedium : Styling.ConditionalStyles.OnScreenMedium { }

public class OnScreenSmall : Styling.ConditionalStyles.OnScreenSmall { }

public class OnScreenXl : Styling.ConditionalStyles.OnScreenXl { }

public class OnScreenXxl : Styling.ConditionalStyles.OnScreenXxl { }

public class Pressed : Styling.ConditionalStyles.Pressed { }

public class Selected : Styling.ConditionalStyles.Selected { }

public class Unchecked : Styling.ConditionalStyles.Unchecked { }

// it seems that Xaml intellisense does not work with this approach..
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
