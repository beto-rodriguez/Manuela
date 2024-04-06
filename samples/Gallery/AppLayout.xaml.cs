using Manuela;
using Manuela.Theming;
using Manuela.WindowStyle;
using MauiIcons.Core;

namespace Gallery;
public partial class AppLayout : AppPage
{
    public AppLayout()
    {
        InitializeComponent();

        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();
    }

    protected override void OnAppLoaded(object? sender, EventArgs e)
    {
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

        var topColor = colorSet.Colors[UIBrush.Gray | UIBrush.Swatch100];
        var bottomColor = colorSet.Colors[UIBrush.Gray | UIBrush.Swatch200];

        ManuelaWindow.SetWindowColors(topColor, bottomColor);
    }
}
