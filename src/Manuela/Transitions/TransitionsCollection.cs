// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.Diagnostics.CodeAnalysis;
using Manuela.Styling;

namespace Manuela;

// should be a dictionary? but it seems that Xaml parser is not doing it right (or i don't know how to declare it...)
public class TransitionsCollection : List<Transition>
{
    private Dictionary<VisualElement, HashSet<ManuelaProperty>> _animatedProperties = [];
    private Dictionary<ManuelaProperty, Transition>? _transitions;
    private bool _allFirst = true;

    public TransitionsCollection()
    { }

    public TransitionsCollection(Transition allTransiton)
    {
        AllTransition = allTransiton;
    }

    public Transition? AllTransition { get; set; }

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

        if (!_transitions.TryGetValue(key, out value))
        {
            // if there is no transition for the property,
            // check if there is a global transition

            if (AllTransition is null)
            {
                return false;
            }
            else
            {
                value = AllTransition;
                isFirst = _allFirst;

                _allFirst = false;
                return true;
            }
        }

        return true;
    }

    public void Dispose()
    {
        _animatedProperties = [];
    }
}
