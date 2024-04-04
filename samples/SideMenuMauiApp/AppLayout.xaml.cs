using Manuela;
using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;
using Manuela.Things;
using MauiIcons.Core;

namespace SideMenuMauiApp;

public partial class AppLayout : AppPage
{
    private bool _isMenuInitialized;
    public bool _isMenuOpen = true;

    public AppLayout()
    {
        InitializeComponent();

        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();

        Loaded += (_, _) =>
        {
            UpdatePointerPassthroughRegion();
            SideMenu.Window.SizeChanged += (_, _) => UpdatePointerPassthroughRegion();
        };
    }

    private void ToggleMenu()
    {
        if (!_isMenuInitialized)
        {
            // on large screens the menu is open by default, on smaller screens it is closed
            _isMenuOpen = BodyElement.GetScreenBreakpoint() >= Breakpoint.Lg;
            _isMenuInitialized = true;

            // The GetScreenBreakpoint works only after the window is initialized
            // that is why this is not in the constructor
        }

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

        SideMenu.SetManuelaStyle(_isMenuOpen ? menuOpened : menuClosed);

        // any visual that was a ScreenStyle, can be used to get the current breakpoint.
        // in this case we use the Body, but it also could be the SideMenu.
        var currentBreakpoint = BodyElement.GetScreenBreakpoint();

        if (currentBreakpoint >= Breakpoint.Lg)
        {
            // do not contract the body when the app is on < large screens
            BodyElement.SetManuelaStyle(_isMenuOpen ? bodyContracted : bodyExpanded);
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

        ManuelaThings.SetPointerPassthroughRegion([new(0, 0, width, 32)]);
    }
}
