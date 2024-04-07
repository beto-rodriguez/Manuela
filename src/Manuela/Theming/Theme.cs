namespace Manuela.Theming;

public class Theme
{
    public Theme(
        UISizeDictionary<double>? spacing = null,
        UISizeDictionary<double>? sizes = null,
        AppThemeBindingDictionary<Shadow>? shadows = null)
    {
        Spacing = spacing ?? new()
        {
            [UISize.None] = 0,
            [UISize.Xs] = 2,
            [UISize.Sm] = 4,
            [UISize.Md] = 8,
            [UISize.Lg] = 12,
            [UISize.Xl] = 18,
            [UISize.Xxl] = 32,
            [UISize.Huge] = 48,
            [UISize.Giant] = 64,
            [UISize.Titanic] = 128
        };

        Sizes = sizes ?? new()
        {
            [UISize.None] = 0,
            [UISize.Xs] = 4,
            [UISize.Sm] = 8,
            [UISize.Md] = 16,
            [UISize.Lg] = 32,
            [UISize.Xl] = 64,
            [UISize.Xxl] = 128,
            [UISize.Huge] = 192,
            [UISize.Giant] = 256,
            [UISize.Titanic] = 512
        };

        Shadows = shadows ?? new()
        {
            [UISize.None] = new( // sad workaround, because AppThemeBinding does not support nulls.
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 0), Radius = 0, Opacity = 0f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 0), Radius = 0, Opacity = 0f }),
            [UISize.Xs] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(2, 2), Radius = 4, Opacity = 0.1f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(2, 2), Radius = 4, Opacity = 0.9f }),
            [UISize.Sm] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 6, Opacity = 0.15f },
                               new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 6, Opacity = 0.9f }),
            [UISize.Md] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 10, Opacity = 0.20f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 3), Radius = 10, Opacity = 0.9f }),
            [UISize.Lg] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 4), Radius = 15, Opacity = 0.225f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(3, 4), Radius = 15, Opacity = 0.9f }),
            [UISize.Xl] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(6, 10), Radius = 20, Opacity = 0.25f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(6, 10), Radius = 20, Opacity = 0.9f }),
            [UISize.Xxl] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 30, Opacity = 0.3f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 30, Opacity = 0.9f }),
            [UISize.Huge] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 40, Opacity = 0.3f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(8, 12), Radius = 40, Opacity = 0.95f }),
            [UISize.Giant] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 50, Opacity = 0.5f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 50, Opacity = 1f }),
            [UISize.Titanic] = new(
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 65, Opacity = 0.6f },
                new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(12, 15), Radius = 65, Opacity = 1f })
        };

        PropertyMap = new()
        {
            ["Margin"] = Spacing,
            ["Padding"] = Spacing,
            ["Shadow"] = Shadows,
            ["Width"] = Sizes,
        };
    }

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

    public Dictionary<string, ISizeSource> PropertyMap { get; }
    public UISizeDictionary<double> Spacing { get; }
    public UISizeDictionary<double> Sizes { get; }
    public AppThemeBindingDictionary<Shadow> Shadows { get; }

    public ThemeParams Params { get; set; } = new();

    public ThemeParams? ButtonParams { get; set; } = new() { BorderWidth = 0, Padding = new(14, 10) };
}
