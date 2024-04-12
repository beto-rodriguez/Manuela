// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class Hovered : ConditionalStyle
{
    private View? _element;
    private Behaviors.Behavior? _behavior;

    public Hovered()
    {
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsHoverStateProperty));
    }

    public override void Dispose(VisualElement visualElement)
    {
        _behavior?.Dispose();
        _element = null;
        base.Dispose(visualElement);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        if (visualElement is not View view)
            throw new Exception(
                $"{nameof(Hovered)} trigger is not supported in elements of type {visualElement.GetType()}. " +
                $"The type does not inherit from {nameof(View)}");

        _element = view;

        _behavior = new Behaviors.Behavior(visualElement);
        _behavior.Enter += () => visualElement.SetValue(Has.IsHoverStateProperty, true);
        _behavior.Exit += () => visualElement.SetValue(Has.IsHoverStateProperty, false);

        Condition.Triggers = [new(visualElement, [Has.IsHoverStateProperty.PropertyName])];
    }
}
