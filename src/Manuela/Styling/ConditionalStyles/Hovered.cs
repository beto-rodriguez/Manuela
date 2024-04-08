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
        Condition = new(visualElement => (bool)visualElement.GetValue(Has.IsHoverStateProperty))
        {
            Triggers = v =>
            {
                if (v is not View view)
                    throw new Exception(
                        $"{nameof(Hovered)} trigger is not supported in elements of type {v.GetType()}. " +
                        $"The type does not inherit from {nameof(View)}");

                _element = view;

                _behavior = new Behaviors.Behavior(v);
                _behavior.Enter += () =>
                    v.SetValue(Has.IsHoverStateProperty, true);
                _behavior.Exit += () => v.SetValue(Has.IsHoverStateProperty, false);

                return [new(v, ["IsHoverState"])];
            }
        };
    }

    public override void Dispose()
    {
        _behavior?.Dispose();
        _element = null;
        base.Dispose();
    }
}
