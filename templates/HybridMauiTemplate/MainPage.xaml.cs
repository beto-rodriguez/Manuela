using Manuela;
using Manuela.WindowStyle;

namespace HybridMauiTemplate;

public partial class MainPage : AppPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppLoaded(object? sender, EventArgs e)
    {
        SetStatusAndNavigationBarColors();
        UpdatePointerPassthroughRegion();
    }

    private static void SetStatusAndNavigationBarColors()
    {
        ManuelaWindow.SetWindowColors(Color.FromArgb("#3a0647"), Color.FromArgb("#ffffff"));
    }

    private void UpdatePointerPassthroughRegion()
    {
        // declares a zone where the pointer events will pass through the title bar on windows
        // lets allow the user to click on the toggle button
        ManuelaWindow.SetPointerPassthroughRegion([new(0, 0, 80, 32)]);
    }
}
