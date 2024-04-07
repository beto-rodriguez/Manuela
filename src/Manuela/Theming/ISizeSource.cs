namespace Manuela.Theming;

public interface ISizeSource
{
    object? Get(UISize size, IServiceProvider serviceProvider);
}
