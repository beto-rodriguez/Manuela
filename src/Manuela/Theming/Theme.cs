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

    public Dictionary<UISize, Shadow> ShadowLight { get; set; } = new()
    {
        [UISize.Xs] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 1), Radius = 2, Opacity = 0.1f },
        [UISize.Sm] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 1), Radius = 3, Opacity = 0.1f },
        [UISize.Md] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 4), Radius = 6, Opacity = 0.1f },
        [UISize.Lg] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 10), Radius = 15, Opacity = 0.1f },
        [UISize.Xl] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 20), Radius = 25, Opacity = 0.2f },
        [UISize.Huge] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 25), Radius = 50, Opacity = 0.25f },
        [UISize.Enormous] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 25), Radius = 75, Opacity = 0.25f },
        [UISize.Giant] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 25), Radius = 100, Opacity = 0.25f },
    };

    public Dictionary<UISize, Shadow> ShadowDark { get; set; } = new()
    {
        [UISize.Xs] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 1), Radius = 2, Opacity = 0.85f },
        [UISize.Sm] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 1), Radius = 3, Opacity = 0.85f },
        [UISize.Md] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 4), Radius = 6, Opacity = 0.85f },
        [UISize.Lg] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 10), Radius = 15, Opacity = 0.85f },
        [UISize.Xl] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 20), Radius = 25, Opacity = 0.9f },
        [UISize.Huge] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 25), Radius = 50, Opacity = 0.95f },
        [UISize.Enormous] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 25), Radius = 75, Opacity = 0.95f },
        [UISize.Giant] = new() { Brush = new SolidColorBrush(Colors.Black), Offset = new(0, 25), Radius = 100, Opacity = 0.95f }
    };
}
