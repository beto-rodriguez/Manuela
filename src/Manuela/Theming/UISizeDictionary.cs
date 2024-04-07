namespace Manuela.Theming;

public class UISizeDictionary<T> : Dictionary<UISize, T>, ISizeSource
{
    public object? Get(UISize size, IServiceProvider serviceProvider)
    {
        return this[size];
    }
}
