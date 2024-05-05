using Manuela.Styling;
using MauiIcons.Core;

namespace Gallery.Layout;
public partial class AppMenu : Border
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
            ActivateItem((StackLayout)MenuStackLayout.Children[0]);
            Window.Page!.SizeChanged += (_, _) => UpdateIndicator(null, false);

            foreach (var item in MenuStackLayout.Children.OfType<MenuItem>())
            {
                item.Tapped += ActivateItem;

                if (item.Display != "More")
                {
                    item.Tapped += CloseMoreMenu;
                }
            }
        };

#if MACCATALYST
        SizeChanged += (s, e) =>
        {
            // scene.FullScreen is only available on 16.0 and later
            // it means that full screen is not displayed properly on earlier versions
            // unless we find a way to get the full screen status for those versions.

            if (!OperatingSystem.IsMacCatalystVersionAtLeast(16)) return;

            var window = (UIKit.UIWindow?)Window.Handler.PlatformView;
            var scene = window?.WindowScene;
            if (scene is null) return;

            Thickness margin = scene.FullScreen
                ? new(0)
                : new(0, 30, 0, 0);
        };
#endif
    }

    public AppMenuMoreOptions MoreOptionsMenu { get; set; } = null!;

    private void ActivateItem(StackLayout clickedVisual)
    {
        // clear the previous selected icon and label
        _activeIcon?.SetCustomState(null);
        _activeLabel?.SetCustomState(null);

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
        _activeVisual = clickedVisual;

        var size = clickedVisual.Measure(double.PositiveInfinity, double.PositiveInfinity).Request;

        // sets and animates the indicator based on the transition defined in XAML

        var scale = .5;
        Indicator.Scale = scale;

        if (MenuStackLayout.Orientation == StackOrientation.Vertical)
        {
            Indicator.TranslationX = 2;
            Indicator.HeightRequest = size.Height;
            Indicator.WidthRequest = 6;
            Indicator.SetManuelaProperty(
                ManuelaProperty.TranslateY,
                clickedIndex * size.Height, null, animated);
        }
        else
        {
            var menuStackSizeRequest = MenuStackLayout
                .Measure(double.PositiveInfinity, double.PositiveInfinity).Request;

            var w = Window.Width - menuStackSizeRequest.Width;
#if WINDOWS
            //  for a reason on windows it seems wrong by about 10px
            // maybe window.width is not the right value to use
            w -= 10 * (1 + scale);
#endif
            Indicator.TranslationY = 1;
            Indicator.WidthRequest = size.Width;
            Indicator.HeightRequest = 6;
            Indicator.SetManuelaProperty(
                ManuelaProperty.TranslateX,
                clickedIndex * size.Width + w * 0.5, null, animated);
        }
    }

    private void CloseMoreMenu(StackLayout clickedVisual)
    {
        MoreOptionsMenu.Close();
    }

    private void OnMoreTapped(StackLayout obj)
    {
        MoreOptionsMenu.Toggle();
    }
}
