using Manuela;
using Manuela.Theming;
using Manuela.WindowStyle;

namespace EmptyApp;

public partial class AppLayout : AppPage
{
    public AppLayout()
    {
        InitializeComponent();
    }

    protected override void OnAppLoaded(object? sender, EventArgs e)
    {
        SetStatusAndNavigationBarColors();
        UpdatePointerPassthroughRegion();

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

    private void UpdatePointerPassthroughRegion()
    {
        // declares a zone where the pointer events will pass through the title bar on windows
        // in this case NavButtons.Width x 32px (32 the windows title bar height)

        ManuelaWindow.SetPointerPassthroughRegion([new(0, 0, (int)NavButtons.Width, 32)]);
    }
}
