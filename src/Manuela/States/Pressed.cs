// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.ComponentModel;
using Manuela.States;

namespace Manuela;

public class Pressed : ConditionalStyle
{
    private VisualElement? _element;
    private PropertyChangedEventHandler? _propertyChangedEventHandler;
    private Behaviors.Behavior? _behavior;

    public Pressed()
    {
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsPressedStateProperty));
    }

    public override void Dispose(VisualElement visualElement)
    {
        _behavior?.Dispose();

        if (_element is not null)
            _element.PropertyChanged -= _propertyChangedEventHandler;

        _element = null;

        base.Dispose(visualElement);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        _element = visualElement;

#if DEBUG
        if (visualElement is not View view)
            throw new Exception(
                $"{nameof(Pressed)} trigger is not supported in elements of type {visualElement.GetType()}. " +
                $"The type does not inherit from {nameof(View)}");
#endif

        _behavior = new Behaviors.Behavior(visualElement);

        _behavior.Down += () => visualElement.SetValue(Has.IsPressedStateProperty, true);
        _behavior.Up += () => visualElement.SetValue(Has.IsPressedStateProperty, false);

        Condition.Triggers = [new(visualElement, [Has.IsPressedStateProperty.PropertyName])];
    }
}
