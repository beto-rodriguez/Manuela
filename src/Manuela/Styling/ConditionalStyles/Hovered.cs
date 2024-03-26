namespace Manuela.Styling.ConditionalStyles;

public class Hovered : ConditionalStyle
{
    public Hovered()
    {
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsHoverStateProperty))
        {
            Triggers = v =>
            {
#if DEBUG
                if (v is not View view)
                    throw new Exception(
                        $"{nameof(Hovered)} trigger is not supported in elements of type {v.GetType()}. " +
                        $"The type does not inherit from {nameof(View)}");
#endif

                var pointerRecognizer = new PointerGestureRecognizer();

                pointerRecognizer.PointerEntered += (sender, e) =>
                {
                    if (sender is null || sender is not BindableObject bindable) return;
                    bindable.SetValue(Has.IsHoverStateProperty, true);
                };

                pointerRecognizer.PointerExited += (sender, e) =>
                {
                    if (sender is null || sender is not BindableObject bindable) return;
                    bindable.SetValue(Has.IsHoverStateProperty, false);
                };

                view.GestureRecognizers.Add(pointerRecognizer);

                return [new(v, ["IsHoverState"])];
            }
        };
    }
}
