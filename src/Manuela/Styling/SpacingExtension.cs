// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.Reflection;
using Manuela.Theming;

namespace Manuela;

[ContentProperty(nameof(Size))]
public class SpacingExtension : IMarkupExtension<Thickness>
{
    public SpacingExtension()
    { }

    public SpacingExtension(UISize size)
    {
        Size = size;
    }

    public UISize? Size { get; set; }

    public UISize? Vertical { get; set; }
    public UISize? Horizontal { get; set; }

    public UISize? Left { get; set; }
    public UISize? Top { get; set; }
    public UISize? Right { get; set; }
    public UISize? Bottom { get; set; }

    public Thickness ProvideValue(IServiceProvider serviceProvider)
    {
        var t = new Thickness();

        // Applies the properties from the less specific to the most specific.

        if (Size is not null) t.Left = t.Top = t.Right = t.Bottom = Theme.Current.Space[Size.Value];

        if (Vertical is not null) t.Top = t.Bottom = Theme.Current.Space[Vertical.Value];
        if (Horizontal is not null) t.Left = t.Right = Theme.Current.Space[Horizontal.Value];

        if (Left is not null) t.Left = Theme.Current.Space[Left.Value];
        if (Top is not null) t.Top = Theme.Current.Space[Top.Value];
        if (Right is not null) t.Right = Theme.Current.Space[Right.Value];
        if (Bottom is not null) t.Bottom = Theme.Current.Space[Bottom.Value];

        return t;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
