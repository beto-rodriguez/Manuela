// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Theming;
using Manuela.Things;

namespace Manuela;

[ContentProperty(nameof(UIBrush))]
public class ColorExtension : IMarkupExtension<BindingBase>
{
    public ColorExtension()
    { }

    public ColorExtension(UIColor color)
    {
        Key = color;
    }

    public UIColor Key { get; set; }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        var flags = (int)Key;

        IMarkupExtension<BindingBase> binding = new AppThemeBindingExtension
        {
            Light = ManuelaThings.ConvertToColor(flags, Theme.Current.LightColors),
            Dark = ManuelaThings.ConvertToColor(flags, Theme.Current.DarkColors)
        };

        return binding.ProvideValue(serviceProvider);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
