using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;

namespace MauiApp1;

public partial class App : Application
{
    public bool _isMenuOpen = true;

    public App()
    {
        InitializeComponent();
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
