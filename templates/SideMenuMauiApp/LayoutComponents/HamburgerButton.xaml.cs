namespace SideMenuMauiApp.LayoutComponents;

public partial class HamburgerButton : Border
{
    public HamburgerButton()
    {
        InitializeComponent();
    }

    public event Action? Tapped;

    private void OnTapped(object sender, TappedEventArgs e)
    {
        Tapped?.Invoke();
    }
}
