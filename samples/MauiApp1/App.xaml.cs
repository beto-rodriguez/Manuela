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
        // in this case 300x32px (300 the side menu width, 32 the windows title bar height)
        ManuelaThings.SetPointerPassthroughRegion([new(0, 0, 300, 32)]);
    }

    private void Button_Clicked(object sender, EventArgs e)
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

        if (Body.GetScreenBreakpoint() >= Breakpoint.Lg)
        {
            // do not contract the body when the app is on < large screens
            Body.SetManuelaStyle(_isMenuOpen ? bodyContracted : bodyExpanded);
        }
    }
}
