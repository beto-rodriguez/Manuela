namespace Manuela.Theming;

public class ColorSet(Dictionary<UIBrush, Color> colors, Color background, Color? backgroundContrast = null)
{
    public Dictionary<UIBrush, Color> Colors { get; } = colors;
    public Color Background { get; set; } = background;
    public Color BackgroundContrast { get; set; } = backgroundContrast ?? background.InvertColor();
}
