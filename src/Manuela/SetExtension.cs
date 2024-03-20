namespace Manuela;

public class SetExtension : Set, IMarkupExtension<Set>
{
    public Set ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
