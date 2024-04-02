using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;
using MauiIcons.Core;

namespace ManuelaApp.Layout;

public partial class AppMenu : ContentView
{
    private MauiIcon? _activeIcon;
    private Label? _activeLabel;
    private int _activeIndex;

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
        clickedVisual ??= (StackLayout)MenuStackLayout.Children[_activeIndex];

        var clickedIndex = _activeIndex = MenuStackLayout.Children.IndexOf(clickedVisual);

        // sets and animates the indicator based on the transition defined in XAML

        if (MenuStackLayout.GetScreenBreakpoint() >= Breakpoint.Md)
        {
            Indicator.SetManuelaProperty(
                ManuelaProperty.AbsoluteLayoutBounds,
                new Rect(10, clickedIndex * clickedVisual.Height + 5, 40, 40),
                animated);
        }
        else
        {
            var w = Window.Width * 0.5 - clickedVisual.Width * MenuStackLayout.Children.Count * 0.5;
            Indicator.SetManuelaProperty(
                ManuelaProperty.AbsoluteLayoutBounds,
                new Rect(w + clickedIndex * clickedVisual.Width + 8, 2, 40, 40),
                animated);
        }
    }
}
