using System.Diagnostics.CodeAnalysis;
using Manuela.Styling;

namespace Manuela.Transitions;

// should be a dictionary? but it seems that Xaml parser is not doing it right (or i don't know how to declare it...)
public class TransitionsCollection : List<Transition>
{
    private readonly HashSet<ManuelaProperty> _animatedProperties = [];
    private Dictionary<ManuelaProperty, Transition>? _transitions;

    public bool TryGetValue(ManuelaProperty key, [MaybeNullWhen(false)] out Transition value, out bool isFirst)
    {
        _transitions ??= this.ToDictionary(x => x.Property, x => x);
        isFirst = _animatedProperties.Add(key);
        return _transitions.TryGetValue(key, out value);
    }
}
