// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Theming;

namespace Manuela;

[ContentProperty(nameof(Size))]
public class ShadowExtension : IMarkupExtension<BindingBase>
{
    public ShadowExtension()
    { }

    public ShadowExtension(UISize size)
    {
        Size = size;
    }

    public UISize Size { get; set; }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        IMarkupExtension<BindingBase> binding = new AppThemeBindingExtension
        {
            Light = Theme.Current.LightShadows[Size],
            Dark = Theme.Current.DarkShadows[Size]
        };

        return binding.ProvideValue(serviceProvider);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
