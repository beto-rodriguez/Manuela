namespace Manuela.Theming.SizeProviders;

public class UISizeDictionary<T> : Dictionary<UISize, T>, ISizeSource
{
    public object? Get(UISize size, object? targetObject, IServiceProvider serviceProvider)
    {
        return this[size];
    }
}
