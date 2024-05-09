using Manuela;
using Manuela.Styling;
using Manuela.States.Screen;
using Manuela.Theming;
using Manuela.WindowStyle;
using MauiIcons.Core;

namespace SideMenuMauiApp;

public partial class AppLayout : AppPage
{
    private bool _isAnimated = false;
    private bool _isMenuOpen = true;

    public AppLayout()
    {
        InitializeComponent();

        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();
    }

    protected override void OnAppLoaded(object? sender, EventArgs e)
    {
        UpdatePointerPassthroughRegion();
        SideMenu.Window.SizeChanged += (_, _) => UpdatePointerPassthroughRegion();

        // the app starts open, but on < lg screens it will be closed
        var isSmall = BodyElement.GetScreenBreakpoint() < Breakpoint.Lg;
        if (isSmall) ToggleMenu();
        _isAnimated = true;

        SetStatusAndNavigationBarColors();

        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += (_, _) => SetStatusAndNavigationBarColors();
    }

    private static void SetStatusAndNavigationBarColors()
    {
        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var colorSet = theme == AppTheme.Light
            ? Theme.Current.LightColors
            : Theme.Current.DarkColors;

        var color = colorSet.Colors[UIBrush.Gray | UIBrush.Swatch100];

        ManuelaWindow.SetWindowColors(color, color);
    }

    private void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;

        var resources = Application.Current!.Resources;

        if (
            !resources.TryGetValue("MenuClosedStyle", out var menuClosed) ||
            !resources.TryGetValue("MenuOpenedStyle", out var menuOpened) ||
            !resources.TryGetValue("BodyContractedStyle", out var bodyContracted) ||
            !resources.TryGetValue("BodyExpandedStyle", out var bodyExpanded)
            )
        {
            throw new Exception("Unable to find menu resources");
        }

        SideMenu.SetManuelaStyle(_isMenuOpen ? menuOpened : menuClosed, _isAnimated);

        // any visual that was a ScreenStyle, can be used to get the current breakpoint.
        // in this case we use the Body, but it also could be the SideMenu.
        var currentBreakpoint = BodyElement.GetScreenBreakpoint();

        if (currentBreakpoint >= Breakpoint.Lg)
        {
            // do not contract the body when the app is on < large screens
            BodyElement.SetManuelaStyle(_isMenuOpen ? bodyContracted : bodyExpanded, _isAnimated);
        }

        ShadowElement.IsVisible = currentBreakpoint < Breakpoint.Lg && _isMenuOpen;

        // update the pointer passthrough region
        UpdatePointerPassthroughRegion();
    }

    private void AppMenuItemTapped()
    {
        // close the menu on < lg screens
        if (BodyElement.GetScreenBreakpoint() < Breakpoint.Lg)
            ToggleMenu();
    }

    private void UpdatePointerPassthroughRegion()
    {
        // declares a zone where the pointer events will pass through the title bar on windows
        // in this case 360x32px (300 the side menu width + 60 the hamburger menu x 32 the windows title bar height)

        var width = (_isMenuOpen || BodyElement.Window.Width >= (int)Breakpoint.Lg) ? 360 : 60;
        ManuelaWindow.SetPointerPassthroughRegion([new(0, 0, width, 32)]);
    }
}
