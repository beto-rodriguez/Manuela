namespace Manuela.Theming;

public class ThemeParams
{
    public string FontFamily { get; set; } = "OpenSansRegular";
    public double FontSize { get; set; } = 15;

    public double BorderWidth { get; set; }
    public Func<Color, Color> BorderColor { get; set; } = c => c;

    public double CornerRadius { get; set; } = 8;

    public Thickness Padding { get; set; } = new();
    public double InputMinimumHeightRequest { get; set; } = 44;
    public double InputMinimumWidthRequest { get; set; } = 44;
}
