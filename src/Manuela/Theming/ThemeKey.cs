namespace Manuela.Theming;

public class ThemeKey<T>(T light, T dark)
{
    public T Light { get; } = light;
    public T Dark { get; } = dark;
}
