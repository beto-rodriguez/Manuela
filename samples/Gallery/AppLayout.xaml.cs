using Manuela;
using Manuela.States.Screen;
using Manuela.Styling;
using Manuela.Theming;
using Manuela.WindowStyle;
using MauiIcons.Core;

namespace Gallery;

public partial class AppLayout : AppPage
{
    public AppLayout()
    {
        InitializeComponent();

        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();
    }

    public override void ShowModal(View view)
    {
        modal.TranslateTo(0, 0, 300);
    }

    protected override void OnAppLoaded(object? sender, EventArgs e)
    {
        SetStatusAndNavigationBarColors();

        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += (_, _) => SetStatusAndNavigationBarColors();

        UpdatePointerPassthroughRegion();
        SizeChanged += (_, _) => UpdatePointerPassthroughRegion();
    }

    private static void SetStatusAndNavigationBarColors()
    {
        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var colorSet = theme == AppTheme.Light
            ? Theme.Current.LightColors
            : Theme.Current.DarkColors;

        var topColor = colorSet.Colors[UIBrush.Gray | UIBrush.Swatch100];
        var bottomColor = colorSet.Colors[UIBrush.Gray | UIBrush.Swatch50];

        ManuelaWindow.SetWindowColors(topColor, bottomColor);
    }

    private void UpdatePointerPassthroughRegion()
    {
        // this method has effect only on windows
        // we let WindosAppSdk that we need to pass the pointer event to the buttons on the
        // top of the window, the user and bell icons.
        // tyhe section needs to be updated manually if you add more buttons to the top bar.

        var w = 40; // the icon width
        var mr = 135; // the margin right, to prevent collase with the window buttons

        ManuelaWindow.SetPointerPassthroughRegion(
            [new((int)Width - mr - w * 2 - 5, 0, w * 2, 32)]);
    }

    private bool _isTopBarShown = true;
    private double _lastScrollY;
    private void OnAppScrolled(object sender, ScrolledEventArgs e)
    {
        if (this.GetScreenBreakpoint() >= Breakpoint.Md) return;

        // on sm and xs screens, we hide the top bar on scroll down

        var deltaScroll = e.ScrollY - _lastScrollY;

        if (deltaScroll > 0)
        {
            if (_isTopBarShown)
            {
                AppTopBar.SetManuelaProperty(ManuelaProperty.TranslateY, -AppTopBar.Height);
                _isTopBarShown = false;
            }
        }
        else
        {
            if (!_isTopBarShown)
            {
                AppTopBar.SetManuelaProperty(ManuelaProperty.TranslateY, 0d);
                _isTopBarShown = true;
            }
        }

        _lastScrollY = e.ScrollY;
    }
}
