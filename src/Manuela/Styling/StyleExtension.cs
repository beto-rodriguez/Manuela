namespace Manuela.Styling;

public class StyleExtension : Style, IMarkupExtension<Style>
{
    public Style ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
