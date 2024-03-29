namespace Manuela.Styling.ConditionalStyles;

public class Hovered : ConditionalStyle
{
    private View? _element;
    private PointerGestureRecognizer? _pointerRecognizer;

    public Hovered()
    {
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsHoverStateProperty))
        {
            Triggers = v =>
            {
                if (v is not View view)
                    throw new Exception(
                        $"{nameof(Hovered)} trigger is not supported in elements of type {v.GetType()}. " +
                        $"The type does not inherit from {nameof(View)}");

                _element = view;

                _pointerRecognizer = new PointerGestureRecognizer();

                _pointerRecognizer.PointerEntered += (sender, e) =>
                {
                    if (sender is null || sender is not BindableObject bindable) return;
                    bindable.SetValue(Has.IsHoverStateProperty, true);
                };

                _pointerRecognizer.PointerExited += (sender, e) =>
                {
                    if (sender is null || sender is not BindableObject bindable) return;
                    bindable.SetValue(Has.IsHoverStateProperty, false);
                };

                view.GestureRecognizers.Add(_pointerRecognizer);

                return [new(v, ["IsHoverState"])];
            }
        };
    }

    public override void Dispose()
    {
        if (_element is not null)
            _ = _element.GestureRecognizers.Remove(_pointerRecognizer);

        _element = null;

        base.Dispose();
    }
}
