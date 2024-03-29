// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

namespace Manuela;

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

            if (bindable is ImageButton imgButton)
            {
                imgButton.Clicked += (_, _) => AppRouting.Routing.GoTo(route);
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
