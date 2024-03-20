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

    public Dictionary<UISize, double> TextSizes { get; set; } = new()
    {
        [UISize.Xs] = 10,
        [UISize.Sm] = 12,
        [UISize.Md] = 14,
        [UISize.Lg] = 18,
        [UISize.Xl] = 22,
        [UISize.Huge] = 28,
        [UISize.Enormous] = 32,
        [UISize.Giant] = 64
    };

    public Dictionary<UISize, double> TextLineHeights { get; set; } = new()
    {
        [UISize.Xs] = 1,
        [UISize.Sm] = 1.1,
        [UISize.Md] = 1.25,
        [UISize.Lg] = 1.6,
        [UISize.Xl] = 1.8,
        [UISize.Huge] = 2,
        [UISize.Enormous] = 2.5,
        [UISize.Giant] = 3
    };

    public List<FontAttributes> TextAttributes { get; set; } =
        [FontAttributes.None, FontAttributes.Italic, FontAttributes.Bold];

    public List<TextAlignment> TextAlign { get; set; } =
        [TextAlignment.Start, TextAlignment.Center, TextAlignment.End];

    public List<TextDecorations> TextDecor { get; set; } =
        [TextDecorations.Underline, TextDecorations.Strikethrough, TextDecorations.None];

    public Dictionary<UISize, double> BorderThickness { get; set; } = new()
    {
        [UISize.Xs] = 0.5,
        [UISize.Sm] = 1,
        [UISize.Md] = 2,
        [UISize.Lg] = 4,
        [UISize.Xl] = 8,
        [UISize.Huge] = 12,
        [UISize.Enormous] = 16,
        [UISize.Giant] = 20
    };

    public Dictionary<UISize, double> BorderRadius { get; set; } = new()
    {
        [UISize.Xs] = 2,
        [UISize.Sm] = 4,
        [UISize.Md] = 8,
        [UISize.Lg] = 12,
        [UISize.Xl] = 16,
        [UISize.Huge] = 24,
        [UISize.Enormous] = 32,
        [UISize.Giant] = 999
    };

    public Dictionary<UISize, double> Padding { get; set; } = new()
    {
        [UISize.None] = 0,
        [UISize.Xs] = 4,
        [UISize.Sm] = 8,
        [UISize.Md] = 12,
        [UISize.Lg] = 16,
        [UISize.Xl] = 20,
        [UISize.Huge] = 24,
        [UISize.Enormous] = 32,
        [UISize.Giant] = 48
    };

    public Dictionary<UISize, double> Margin { get; set; } = new()
    {
        [UISize.None] = 0,
        [UISize.Xs] = 4,
        [UISize.Sm] = 8,
        [UISize.Md] = 12,
        [UISize.Lg] = 16,
        [UISize.Xl] = 20,
        [UISize.Huge] = 24,
        [UISize.Enormous] = 32,
        [UISize.Giant] = 48
    };

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
