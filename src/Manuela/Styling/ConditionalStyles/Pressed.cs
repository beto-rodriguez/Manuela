// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.ComponentModel;
using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class Pressed : ConditionalStyle
{
    private VisualElement? _element;
    private PropertyChangedEventHandler? _propertyChangedEventHandler;
    private Behaviors.Behavior? _behavior;

    public Pressed()
    {
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsPressedStateProperty))
        {
            Triggers = v =>
            {
                _element = v;

                // we could just use the GetViewTriggers???
                // probably yes... but lets use the Maui things for now

                if (v is Button button)
                {
                    _propertyChangedEventHandler = (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(Button.IsPressed)))
                            return;

                        v.SetValue(Has.IsPressedStateProperty, button.IsPressed);
                    };

                    button.PropertyChanged += _propertyChangedEventHandler;

                    return [new(v, ["IsPressed"])];
                }

                if (v is ImageButton imageButton)
                {
                    _propertyChangedEventHandler += (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(ImageButton.IsPressed)))
                            return;

                        v.SetValue(Has.IsPressedStateProperty, imageButton.IsPressed);
                    };

                    imageButton.PropertyChanged += _propertyChangedEventHandler;
                }

                return GetViewTriggers()(v);
            }
        };
    }

    public override void Dispose()
    {
        _behavior?.Dispose();

        if (_element is not null)
            _element.PropertyChanged -= _propertyChangedEventHandler;

        _element = null;

        base.Dispose();
    }

    private Func<VisualElement, Expressions.Trigger[]> GetViewTriggers()
    {
        return v =>
        {
#if DEBUG
            if (v is not View view)
                throw new Exception(
                    $"{nameof(Pressed)} trigger is not supported in elements of type {v.GetType()}. " +
                    $"The type does not inherit from {nameof(View)}");
#endif

            _behavior = new Behaviors.Behavior(v);

            _behavior.Down += () => v.SetValue(Has.IsPressedStateProperty, true);
            _behavior.Up += () => v.SetValue(Has.IsPressedStateProperty, false);

            return [new(v, ["IsPressedState"])];
        };
    }
}
