using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;
using Manuela.Things;

namespace MauiApp1;

public partial class App : Application
{
    public bool _isMenuOpen = true;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnStart()
    {
        base.OnStart();

        // declares a zone where the pointer events will pass through the title bar on windows
        // in this case 360x32px (300 the side menu width + 60 the hamburger menu x 32 the windows title bar height)
        ManuelaThings.SetPointerPassthroughRegion([new(0, 0, 360, 32)]);
    }

    private void ToggleMenu(object sender, TappedEventArgs e)
    {
        _isMenuOpen = !_isMenuOpen;

        if (
            !Resources.TryGetValue("MenuClosedStyle", out var menuClosed) ||
            !Resources.TryGetValue("MenuOpenedStyle", out var menuOpened) ||
            !Resources.TryGetValue("BodyContractedStyle", out var bodyContracted) ||
            !Resources.TryGetValue("BodyExpandedStyle", out var bodyExpanded)
            )
        {
            throw new Exception("Unable to find menu resources");
        }

        SideMenu.SetManuelaStyle(_isMenuOpen ? menuOpened : menuClosed);

        // any visual that was a ScreenStyle, can be used to get the current breakpoint.
        // in this case we use the Body, but it also could be the SideMenu.
        var currentBreakpoint = Body.GetScreenBreakpoint();

        if (currentBreakpoint >= Breakpoint.Lg)
        {
            // do not contract the body when the app is on < large screens
            Body.SetManuelaStyle(_isMenuOpen ? bodyContracted : bodyExpanded);
        }

        Shadow.IsVisible = currentBreakpoint < Breakpoint.Lg && _isMenuOpen;

        // update the pointer passthrough region
        ManuelaThings.SetPointerPassthroughRegion([new(0, 0, _isMenuOpen ? 360 : 60, 32)]);
    }

    private void OnPointerReleased(object sender, PointerEventArgs e)
    {
        ToggleMenu(sender, new(null));
    }
}
