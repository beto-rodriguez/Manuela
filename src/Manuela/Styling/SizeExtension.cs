// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.Reflection;
using Manuela.Theming;

namespace Manuela;

[ContentProperty(nameof(Key))]
public class SizeExtension : IMarkupExtension
{
    public SizeExtension()
    { }

    public SizeExtension(UISize size)
    {
        Key = size;
    }

    public UISize Key { get; set; }

    public object? ProvideValue(IServiceProvider serviceProvider)
    {
        var valueProvider = serviceProvider?.GetService<IProvideValueTarget>()
            ?? throw new ArgumentException("Unable to get the IProvideValueTarget service.");

        var bp = valueProvider.TargetProperty as BindableProperty;
        var pi = valueProvider.TargetProperty as PropertyInfo;

        if (valueProvider.TargetObject is Setter setter)
            bp = setter.Property;

        var name = bp?.PropertyName ?? pi?.Name
            ?? throw new Exception("Unable to find a property name");

        if (!Theme.Current.PropertyMap.TryGetValue(name, out var sizeSource))
            throw new Exception($"Property {name} is not supported by the {nameof(SizeExtension)}");

        return sizeSource.Get(Key, valueProvider.TargetObject, serviceProvider);
    }
}
