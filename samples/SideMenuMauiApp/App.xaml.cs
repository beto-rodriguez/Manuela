using Manuela.Things;
using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;

namespace SideMenuMauiApp;

public partial class App : Application
{
    private bool _isMenuInitialized;
    public bool _isMenuOpen = true;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnStart()
    {
        base.OnStart();

        UpdatePointerPassthroughRegion();
        SideMenu.Window.SizeChanged += (_, _) => UpdatePointerPassthroughRegion();
    }

    private void ToggleMenu(object sender, TappedEventArgs e)
    {
        if (!_isMenuInitialized)
        {
            // on large screens the menu is open by default, on smaller screens it is closed
            _isMenuOpen = Body.GetScreenBreakpoint() >= Breakpoint.Lg;
            _isMenuInitialized = true;

            // The GetScreenBreakpoint works only after the window is initialized
            // that is why this is not in the constructor
        }

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
        UpdatePointerPassthroughRegion();
    }

    private void UpdatePointerPassthroughRegion()
    {
        // declares a zone where the pointer events will pass through the title bar on windows
        // in this case 360x32px (300 the side menu width + 60 the hamburger menu x 32 the windows title bar height)

        var width = (_isMenuOpen || Body.Window.Width >= (int)Breakpoint.Lg) ? 360 : 60;

        ManuelaThings.SetPointerPassthroughRegion([new(0, 0, width, 32)]);
    }

    private void OnMenuItemTapped(object sender)
    {
        var a = 1;
    }
}
