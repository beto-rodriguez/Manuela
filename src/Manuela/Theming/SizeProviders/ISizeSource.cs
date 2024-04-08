namespace Manuela.Theming.SizeProviders;

public interface ISizeSource
{
    object? Get(UISize size, object? targetObject, IServiceProvider serviceProvider);
}
