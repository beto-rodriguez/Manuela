namespace Manuela.Theming;

public class ColorSet(Dictionary<UIBrush, Color> colors)
{
    public Dictionary<UIBrush, Color> Colors { get; } = colors;
}
