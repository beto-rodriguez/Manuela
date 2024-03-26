using Manuela.Expressions;

namespace Manuela.Styling.ConditionalStyles;

public class Pressed : ConditionalStyle
{
    public Pressed()
    {
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsPressedStateProperty))
        {
            Triggers = v =>
            {
                if (v is Button button)
                {
                    button.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(Button.IsPressed)))
                            return;

                        v.SetValue(Has.IsPressedStateProperty, button.IsPressed);
                    };

                    return [new(v, ["IsPressed"])];
                }

                if (v is ImageButton imageButton)
                    imageButton.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(ImageButton.IsPressed)))
                            return;

                        v.SetValue(Has.IsPressedStateProperty, imageButton.IsPressed);
                    };

                return GetViewTriggers()(v);
            }
        };
    }

    private Func<VisualElement, ConditionUpdateTrigger[]> GetViewTriggers()
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
