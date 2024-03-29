namespace Manuela.Behaviors;

public class ManuelaTappedGesture : GestureRecognizer
{
    private Element? _element;
    private Behavior? _behavior;

    public event Action<object?>? Tapped;

    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent is null)
        {
            _behavior?.Dispose();
            _behavior = null;
            _element = null;
        }
        else
        {
            if (Parent is not VisualElement ve) return;

            _element = ve;
            _behavior = new Behavior(ve);
            _behavior.Up += OnUp;
        }
    }

    private void OnUp()
    {
        Tapped?.Invoke(_element);
    }
}
