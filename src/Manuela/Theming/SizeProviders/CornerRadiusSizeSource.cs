namespace Manuela.Theming.SizeProviders;

public class CornerRadiusSizeSource : Dictionary<UISize, int>, ISizeSource
{
    public object? Get(UISize size, object? targetObject, IServiceProvider serviceProvider)
    {
        var sizeValue = this[size];

        // at least RopundedRectangle use CornerRadius instead of double (I hope all shapes do or will ever do.).
        if (targetObject is IShape) return new CornerRadius(sizeValue);

        return sizeValue;
    }
}
