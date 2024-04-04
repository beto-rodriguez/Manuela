namespace SideMenuMauiApp.Layout;

public partial class Shadow : Border
{
    public Shadow()
    {
        InitializeComponent();
    }

    public event Action? Tapped;

    private void OnShadowTapped(object sender, TappedEventArgs e)
    {
        Tapped?.Invoke();
    }
}
