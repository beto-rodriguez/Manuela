using System.Diagnostics.CodeAnalysis;
using Manuela.Styling;

namespace Manuela.Transitions;

// should be a dictionary? but it seems that Xaml parser is not doing it right (or i don't know how to declare it...)
public class TransitionsCollection : List<Transition>
{
    private Dictionary<VisualElement, HashSet<ManuelaProperty>> _animatedProperties = [];
    private Dictionary<ManuelaProperty, Transition>? _transitions;

    public bool TryGetValue(
        VisualElement visualElement,
        ManuelaProperty key,
        [MaybeNullWhen(false)] out Transition value,
        out bool isFirst)
    {
        _transitions ??= this.ToDictionary(x => x.Property, x => x);

        if (!_animatedProperties.TryGetValue(visualElement, out var visualAnimatedProperties))
            _animatedProperties.Add(visualElement, visualAnimatedProperties = []);

        isFirst = visualAnimatedProperties.Add(key);

        return _transitions.TryGetValue(key, out value);
    }

    public void Dispose()
    {
        _animatedProperties = [];
    }
}
