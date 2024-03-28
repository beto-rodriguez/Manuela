using System.ComponentModel;

namespace Manuela.Styling.ConditionalStyles;

public class Pressed : ConditionalStyle
{
    private VisualElement? _element;
    private PropertyChangedEventHandler? _propertyChangedEventHandler;

    public Pressed()
    {
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsPressedStateProperty))
        {
            Triggers = v =>
            {
                _element = v;

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

            var pointerRecognizer = new PointerGestureRecognizer();

            pointerRecognizer.PointerPressed += (sender, e) =>
            {
                if (sender is null || sender is not BindableObject bindable) return;
                bindable.SetValue(Has.IsPressedStateProperty, true);
            };

            pointerRecognizer.PointerReleased += (sender, e) =>
            {
                if (sender is null || sender is not BindableObject bindable) return;
                bindable.SetValue(Has.IsPressedStateProperty, false);
            };

            view.GestureRecognizers.Add(pointerRecognizer);

            return [new(v, ["IsPressedState"])];
        };
    }
}
