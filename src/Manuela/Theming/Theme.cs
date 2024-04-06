namespace Manuela.Theming;

public class Theme
{
    public static Theme Current { get; set; } = new();

    public ColorSet LightColors { get; set; } = new(
        colors: ColorPalletes.BuildDictionary(
            ColorPalletes.Blue,
            ColorPalletes.Orange,
            ColorPalletes.Pink,
            ColorPalletes.Slate),
        background: Color.FromRgba("#fafafa"),
        backgroundContrast: Color.FromArgb("#18181b"));

    public ColorSet DarkColors { get; set; } = new(
        colors: ColorPalletes.BuildDictionary(
            ColorPalletes.Blue.Reverse().ToArray(),
            ColorPalletes.Orange.Reverse().ToArray(),
            ColorPalletes.Pink.Reverse().ToArray(),
            ColorPalletes.Slate.Reverse().ToArray()),
        background: Color.FromRgba("#18181b"),
        backgroundContrast: Color.FromArgb("#fafafa"));

    public ThemeParams Params { get; set; } = new();

    public ThemeParams? ButtonParams { get; set; } = new() { BorderWidth = 0, Padding = new(14, 10) };

    public Dictionary<UISize, double> Spacing { get; set; } = new()
    {
        [UISize.None] = 0,
        [UISize.Xs] = 2,
        [UISize.Sm] = 4,
        [UISize.Md] = 8,
        [UISize.Lg] = 12,
        [UISize.Xl] = 16,
        [UISize.Xxl] = 20,
        [UISize.Huge] = 24,
        [UISize.Giant] = 32,
        [UISize.Titanic] = 48
    };

    public Dictionary<UISize, Shadow> LightShadows { get; set; } = new()
    {
        [UISize.None] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 0), Radius = 0, Opacity = 0f }, // sad workaround, because AppThemeBinding does not support nulls.
        [UISize.Xs] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(2, 2), Radius = 4, Opacity = 0.1f },
        [UISize.Sm] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 6, Opacity = 0.15f },
        [UISize.Md] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 10, Opacity = 0.20f },
        [UISize.Lg] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 4), Radius = 15, Opacity = 0.225f },
        [UISize.Xl] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(6, 10), Radius = 20, Opacity = 0.25f },
        [UISize.Xxl] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 30, Opacity = 0.3f },
        [UISize.Huge] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 40, Opacity = 0.3f },
        [UISize.Giant] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 50, Opacity = 0.5f },
        [UISize.Titanic] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 65, Opacity = 0.6f }
    };

    public Dictionary<UISize, Shadow> DarkShadows { get; set; } = new()
    {
        [UISize.None] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 0), Radius = 0, Opacity = 0f }, // sad workaround, because AppThemeBinding does not support nulls.
        [UISize.Xs] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(2, 2), Radius = 4, Opacity = 0.9f },
        [UISize.Sm] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 6, Opacity = 0.9f },
        [UISize.Md] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 10, Opacity = 0.9f },
        [UISize.Lg] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 4), Radius = 15, Opacity = 0.9f },
        [UISize.Xl] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(6, 10), Radius = 20, Opacity = 0.9f },
        [UISize.Xxl] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 30, Opacity = 0.9f },
        [UISize.Huge] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 40, Opacity = 0.95f },
        [UISize.Giant] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 50, Opacity = 1f },
        [UISize.Titanic] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 65, Opacity = 1f }
    };
}
