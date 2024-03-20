
namespace Manuela;

public struct ManuelaStyle
{
    public UIBrush Background { get; set; }
}

internal struct ManuelaStyleSet
{
    public bool IsInitialized;

    public ManuelaStyle All;
    public ManuelaStyle Sm;
    public ManuelaStyle Md;
    public ManuelaStyle Lg;
    public ManuelaStyle Xl;
    public ManuelaStyle Xxl;

    public ManuelaStyleSet SetInitialized(bool value)
    {
        return new ManuelaStyleSet
        {
            IsInitialized = value,
            All = All,
            Sm = Sm,
            Md = Md,
            Lg = Lg,
            Xl = Xl,
            Xxl = Xxl
        };
    }

    public ManuelaStyleSet SetAll(ManuelaStyle style)
    {
        return new ManuelaStyleSet
        {
            IsInitialized = IsInitialized,
            All = style,
            Sm = Sm,
            Md = Md,
            Lg = Lg,
            Xl = Xl,
            Xxl = Xxl
        };
    }

    public ManuelaStyleSet SetSm(ManuelaStyle style)
    {
        return new ManuelaStyleSet
        {
            IsInitialized = IsInitialized,
            All = All,
            Sm = style,
            Md = Md,
            Lg = Lg,
            Xl = Xl,
            Xxl = Xxl
        };
    }

    public ManuelaStyleSet SetMd(ManuelaStyle style)
    {
        return new ManuelaStyleSet
        {
            IsInitialized = IsInitialized,
            All = All,
            Sm = Sm,
            Md = style,
            Lg = Lg,
            Xl = Xl,
            Xxl = Xxl
        };
    }

    public ManuelaStyleSet SetLg(ManuelaStyle style)
    {
        return new ManuelaStyleSet
        {
            IsInitialized = IsInitialized,
            All = All,
            Sm = Sm,
            Md = Md,
            Lg = style,
            Xl = Xl,
            Xxl = Xxl
        };
    }

    public ManuelaStyleSet SetXl(ManuelaStyle style)
    {
        return new ManuelaStyleSet
        {
            IsInitialized = IsInitialized,
            All = All,
            Sm = Sm,
            Md = Md,
            Lg = Lg,
            Xl = style,
            Xxl = Xxl
        };
    }

    public ManuelaStyleSet SetXxl(ManuelaStyle style)
    {
        return new ManuelaStyleSet
        {
            IsInitialized = IsInitialized,
            All = All,
            Sm = Sm,
            Md = Md,
            Lg = Lg,
            Xl = Xl,
            Xxl = style
        };
    }
}

public class ManuelaStyleExtension : IMarkupExtension<ManuelaStyle>
{
    public UIBrush Background { get; set; }

    public ManuelaStyle ProvideValue(IServiceProvider serviceProvider)
    {
        return new ManuelaStyle { Background = Background };
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}

public enum UIBrush
{
    Primary = UICC.Primary,
    Secondary = UICC.Secondary,
    Tertiary = UICC.Tertiary,
    Gray = UICC.Gray
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
