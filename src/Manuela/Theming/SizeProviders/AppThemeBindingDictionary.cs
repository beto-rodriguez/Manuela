namespace Manuela.Theming.SizeProviders;

public class AppThemeBindingDictionary<T> : Dictionary<UISize, ThemeKey<T>>, ISizeSource
{
    public object? Get(UISize size, object? targetObject, IServiceProvider serviceProvider)
    {
        var value = this[size];

        IMarkupExtension<BindingBase> binding = new AppThemeBindingExtension
        {
            Light = value.Light,
            Dark = value.Dark
        };

        return binding.ProvideValue(serviceProvider);
    }
}
