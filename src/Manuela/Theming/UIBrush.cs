namespace Manuela.Theming;

[Flags]
public enum UIBrush
{
    Primary = UICC.Primary,
    Secondary = UICC.Secondary,
    Tertiary = UICC.Tertiary,
    Gray = UICC.Gray,

    Swatch50 = UICC.Sw50,
    Swatch100 = UICC.Sw100,
    Swatch200 = UICC.Sw200,
    Swatch300 = UICC.Sw300,
    Swatch400 = UICC.Sw400,
    Swatch500 = UICC.Sw500,
    Swatch600 = UICC.Sw600,
    Swatch700 = UICC.Sw700,
    Swatch800 = UICC.Sw800,
    Swatch900 = UICC.Sw900,
    Swatch950 = UICC.Sw950,

    Gradient = UICC.Gradient,
    GradientSm = UICC.Gradient | UICC.GradientSmall,
    GradientLg = UICC.Gradient | UICC.GradientLarge,

    GradientX = UICC.Gradient | UICC.GradientX,
    GradientY = UICC.Gradient | UICC.GradientY
}
