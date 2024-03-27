using System.Diagnostics.CodeAnalysis;
using Manuela.Styling;

namespace Manuela.Transitions;

// should be a dictionary? but it seems that Xaml parses is not keeping the keys (or i don't know how...)
public class TransitionsCollection : List<Transition>
{
    private Dictionary<ManuelaProperty, Transition>? _transitions;

    public bool TryGetValue(ManuelaProperty key, [MaybeNullWhen(false)] out Transition value)
    {
        _transitions ??= this.ToDictionary(x => x.Property, x => x);
        return _transitions.TryGetValue(key, out value);
    }
}
