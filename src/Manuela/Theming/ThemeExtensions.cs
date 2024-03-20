namespace Manuela.Theming;

public static class ThemeExtensions
{
    public static Color Shiny(this Color target, Color[] options)
    {
        return options.Select(c => new
        {
            distance =
                Math.Pow(c.Red - target.Red, 2) +
                Math.Pow(c.Green - target.Green, 2) +
                Math.Pow(c.Blue - target.Blue, 2),
            color = c,
        }).MaxBy(x => x.distance)?.color ?? throw new Exception("Shinier color not found.");
    }

    public static Color Tint(this Color color, Color toColor, float factor)
    {
        var r = color.Red + (toColor.Red - color.Red) * factor;
        var g = color.Green + (toColor.Green - color.Green) * factor;
        var b = color.Blue + (toColor.Blue - color.Blue) * factor;

        return Color.FromRgba(r, g, b, color.Alpha);
    }

    public static Color InvertColor(this Color color)
    {
        return Color.FromRgba(1 - color.Red, 1 - color.Green, 1 - color.Blue, color.Alpha);
    }
}
