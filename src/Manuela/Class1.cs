using Manuela.Theming;

namespace Manuela;

public class ManuelaStyleExtension : IMarkupExtension<ManuelaStyle>
{
    private readonly ManuelaStyle _style = [];

    public UIBrush Background { set => _style[ManuelaProperty.Background] = value; }

    public ManuelaStyle ProvideValue(IServiceProvider serviceProvider)
    {
        return _style;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}

public enum UIBrush
{
    Unset = 0,

    Primary = UICC.Primary,
    Secondary = UICC.Secondary,
    Tertiary = UICC.Tertiary,
    Gray = UICC.Gray
}

public enum UIColor
{
    Unset = 0,

    Primary = UICC.Primary,
    Secondary = UICC.Secondary,
    Tertiary = UICC.Tertiary,
    Gray = UICC.Gray
}

public enum UISize
{
    Unset = 0,

    None,
    Xs,
    Sm,
    Md,
    Lg,
    Xl,
    Huge,
    Enormous,
    Giant
}


internal static class UICC // UIColorConstants
{
    public const int Primary = 1 << 0;
    public const int Secondary = 1 << 1;
    public const int Tertiary = 1 << 2;
    public const int Gray = 1 << 3;

    public const int Sw50 = 1 << 10;
    public const int Sw100 = 1 << 11;
    public const int Sw200 = 1 << 12;
    public const int Sw300 = 1 << 13;
    public const int Sw400 = 1 << 14;
    public const int Sw500 = 1 << 15;
    public const int Sw600 = 1 << 16;
    public const int Sw700 = 1 << 17;
    public const int Sw800 = 1 << 18;
    public const int Sw900 = 1 << 19;
    public const int Sw950 = 1 << 20;

    public const int Gradient = 1 << 21;
    public const int GradientSmall = 1 << 23;
    public const int GradientLarge = 1 << 22;

    public const int GradientX = 1 << 24;
    public const int GradientY = 1 << 25;
}
