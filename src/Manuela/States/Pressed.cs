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

        // we could just use the GetViewTriggers???
        // probably yes... but lets use the Maui things for now

        if (visualElement is Button button)
        {
            _propertyChangedEventHandler = (sender, e) =>
            {
                if (e.PropertyName is null or not (nameof(Button.IsPressed)))
                    return;

                visualElement.SetValue(Has.IsPressedStateProperty, button.IsPressed);
            };

            button.PropertyChanged += _propertyChangedEventHandler;
        }
        else if (visualElement is ImageButton imageButton)
        {
            _propertyChangedEventHandler += (sender, e) =>
            {
                if (e.PropertyName is null or not (nameof(ImageButton.IsPressed)))
                    return;

                visualElement.SetValue(Has.IsPressedStateProperty, imageButton.IsPressed);
            };

            imageButton.PropertyChanged += _propertyChangedEventHandler;
        }
        else
        {
#if DEBUG
            if (visualElement is not View view)
                throw new Exception(
                    $"{nameof(Pressed)} trigger is not supported in elements of type {visualElement.GetType()}. " +
                    $"The type does not inherit from {nameof(View)}");
#endif

            _behavior = new Behaviors.Behavior(visualElement);

            _behavior.Down += () => visualElement.SetValue(Has.IsPressedStateProperty, true);
            _behavior.Up += () => visualElement.SetValue(Has.IsPressedStateProperty, false);
        }

        Condition.Triggers = [new(visualElement, [Has.IsPressedStateProperty.PropertyName])];
    }
}
