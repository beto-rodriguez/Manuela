using Manuela.Theming.SizeProviders;

namespace Manuela.Theming;

public class Theme
{
    public Theme(
        UISizeDictionary<double>? spacing = null,
        UISizeDictionary<double>? sizes = null,
        UISizeDictionary<double>? borders = null,
        CornerRadiusSizeSource? radius = null,
        UISizeDictionary<double>? textSize = null,
        UISizeDictionary<double>? lineHeight = null,
        AppThemeBindingDictionary<Shadow>? shadows = null)
    {
        Space = spacing ?? new()
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

        Size = sizes ?? new()
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

        Border = borders ?? new()
        {
            [UISize.None] = 0,
            [UISize.Xs] = 1,
            [UISize.Sm] = 2,
            [UISize.Md] = 4,
            [UISize.Lg] = 6,
            [UISize.Xl] = 8,
            [UISize.Xxl] = 10,
            [UISize.Huge] = 14,
            [UISize.Giant] = 16,
            [UISize.Titanic] = 20
        };

        Radius = radius ?? new()
        {
            [UISize.None] = 0,
            [UISize.Xs] = 1,
            [UISize.Sm] = 2,
            [UISize.Md] = 4,
            [UISize.Lg] = 8,
            [UISize.Xl] = 12,
            [UISize.Xxl] = 16,
            [UISize.Huge] = 24,
            [UISize.Giant] = 32,
            [UISize.Titanic] = 9999 // <- propably not working on IOS
        };

        TextSize = textSize ?? new()
        {
            [UISize.None] = 0,
            [UISize.Xs] = 10,
            [UISize.Sm] = 12,
            [UISize.Md] = 14,
            [UISize.Lg] = 16,
            [UISize.Xl] = 20,
            [UISize.Xxl] = 24,
            [UISize.Huge] = 32,
            [UISize.Giant] = 48,
            [UISize.Titanic] = 64
        };

        LineHeight = lineHeight ?? new()
        {
            [UISize.None] = 1,
            [UISize.Xs] = 1.15,
            [UISize.Sm] = 1.25,
            [UISize.Md] = 1.375,
            [UISize.Lg] = 1.5,
            [UISize.Xl] = 1.625,
            [UISize.Xxl] = 1.75,
            [UISize.Huge] = 1.85,
            [UISize.Giant] = 2,
            [UISize.Titanic] = 2.5
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

        var thicknessSpacing = new UISizeDictionary<Thickness>();
        foreach (var pair in Space)
            thicknessSpacing[pair.Key] = new(pair.Value);

        PropertyMap = new()
        {
            ["Margin"] = thicknessSpacing,
            ["Padding"] = thicknessSpacing,
            ["Spacing"] = Space, // the stacklayout spacing
            ["StrokeThickness"] = Border,
            ["BorderThickness"] = Border,
            ["BorderWidth"] = Border,
            ["StrokeShape"] = Radius,
            ["CornerRadius"] = Radius,
            ["BorderRadius"] = Radius,
            ["FontSize"] = TextSize,
            ["Shadow"] = Shadows,
            ["TextSize"] = TextSize,
            ["LineHeight"] = LineHeight,
            ["Width"] = Size,
            ["Height"] = Size,
            ["MaxWidth"] = Size,
            ["MaxHeight"] = Size,
            ["MinWidth"] = Size,
            ["MinHeight"] = Size,
            ["WidthRequest"] = Size,
            ["HeightRequest"] = Size,
            ["MaximumWidthRequest"] = Size,
            ["MaximumHeightRequest"] = Size,
            ["MinimumWidthRequest"] = Size,
            ["MinimumHeightRequest"] = Size,
        };
    }

    public static Theme Current { get; set; } = new();

    public ColorSet LightColors { get; set; } = new(
        colors: ColorPalletes.BuildDictionary(
            ColorPalletes.Blue,
            ColorPalletes.Green,
            ColorPalletes.Red,
            ColorPalletes.Slate));

    public ColorSet DarkColors { get; set; } = new(
        colors: ColorPalletes.BuildDictionary(
            ColorPalletes.Blue.Reverse().ToArray(),
            ColorPalletes.Green.Reverse().ToArray(),
            ColorPalletes.Red.Reverse().ToArray(),
            ColorPalletes.Gray.Reverse().ToArray()));

    public Dictionary<string, ISizeSource> PropertyMap { get; }
    public UISizeDictionary<double> Space { get; }
    public UISizeDictionary<double> Size { get; }
    public UISizeDictionary<double> Border { get; }
    public CornerRadiusSizeSource Radius { get; } // int because Button.CornerRadius is int
    public UISizeDictionary<double> TextSize { get; }
    public UISizeDictionary<double> LineHeight { get; }
    public AppThemeBindingDictionary<Shadow> Shadows { get; }

    public ThemeParams Params { get; set; } = new();

    public ThemeParams? ButtonParams { get; set; } = new() { BorderWidth = 0, Padding = new(14, 10) };
}
