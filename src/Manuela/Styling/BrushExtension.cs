// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Theming;
using Manuela.Things;

namespace Manuela;

[ContentProperty(nameof(Key))]
public class BrushExtension : IMarkupExtension<BindingBase>
{
    public BrushExtension()
    { }

    public BrushExtension(UIBrush brush)
    {
        Key = brush;
    }

    public UIBrush Key { get; set; }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        var flags = (int)Key;

        IMarkupExtension<BindingBase> binding = new AppThemeBindingExtension
        {
            Light = ManuelaThings.ConvertToBrush(flags, Theme.Current.LightColors),
            Dark = ManuelaThings.ConvertToBrush(flags, Theme.Current.DarkColors)
        };

        return binding.ProvideValue(serviceProvider);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
