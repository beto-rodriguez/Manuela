using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;
using MauiIcons.Core;

namespace Gallery.Layout;
public partial class AppMenu : Grid
{
    private MauiIcon? _activeIcon;
    private Label? _activeLabel;
    private StackLayout? _activeVisual;

    public AppMenu()
    {
        InitializeComponent();

        // activate the first icon and label
        Loaded += (_, _) =>
        {
            TapGestureRecognizer_Tapped(MenuStackLayout.Children[0], null!);
            Window.SizeChanged += (_, _) => UpdateIndicator(null, false);
        };
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        // clear the previous selected icon and label
        _activeIcon?.SetCustomState(null);
        _activeLabel?.SetCustomState(null);

        var clickedVisual = (StackLayout)sender;

        // we know that the clicked visual is an HorizontalStackLayout
        // and we know that it has 2 children, the first is the icon and the second is the label

        var icon = (MauiIcon)clickedVisual.Children[0];
        var label = (Label)clickedVisual.Children[1];

        // mark the clicked icon and label as selected
        icon.SetCustomState("active");
        label.SetCustomState("active");

        _activeIcon = icon;
        _activeLabel = label;

        UpdateIndicator(clickedVisual, true);
    }

    private void UpdateIndicator(StackLayout? clickedVisual, bool animated)
    {
        clickedVisual ??= _activeVisual;
        if (clickedVisual is null) return;

        var clickedIndex = MenuStackLayout.Children.IndexOf(clickedVisual);
        if (clickedIndex == -1)
        {
            clickedIndex =
                MenuDesktopStackLayout.Children.IndexOf(clickedVisual) + MenuStackLayout.Children.Count;
        }

        _activeVisual = clickedVisual;

        // sets and animates the indicator based on the transition defined in XAML

        if (MenuStackLayout.GetScreenBreakpoint() >= Breakpoint.Md)
        {
            Indicator.TranslationX = 0;
            Indicator.HeightRequest = clickedVisual.Height;
            Indicator.WidthRequest = 6;
            Indicator.SetManuelaProperty(
                ManuelaProperty.TranslateY,
                clickedIndex * clickedVisual.Height, animated);
        }
        else
        {
            var w = Window.Width - MenuStackLayout.Width;
#if WINDOWS
            //  for a reason on windows it seems wrong by about 10px
            // maybe window.width is not the right value to use
            w -= 10;
#endif

            Indicator.TranslationY = 0;
            Indicator.WidthRequest = clickedVisual.Width;
            Indicator.HeightRequest = 6;
            Indicator.SetManuelaProperty(
                ManuelaProperty.TranslateX,
                clickedIndex * clickedVisual.Width + w * 0.5, animated);
        }
    }
}
