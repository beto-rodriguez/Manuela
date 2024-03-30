namespace Manuela.Styling.ConditionalStyles.Screen;

public class OnScreenSize : ConditionalStyle
{
    private static readonly Dictionary<OperatorKind, Func<Breakpoint, Breakpoint, bool>> s_comparers = new()
    {
        { OperatorKind.Equals, (p1, p2) => p1 == p2 },
        { OperatorKind.GreaterThanOrEquals, (p1, p2) => p1 >= p2 },
        { OperatorKind.GreaterThan, (p1, p2) => p1 > p2 },
        { OperatorKind.LessThanOrEquals, (p1, p2) => p1 <= p2 },
        { OperatorKind.LessThan, (p1, p2) => p1 < p2 }
    };
    private VisualElement? _element;
    private EventHandler? _windowHandler;

    public OnScreenSize(Breakpoint targetBreakpoint)
    {
        Condition = new(visualElement => Compare(visualElement, targetBreakpoint))
        {
            Triggers = v =>
            {
                _element = v;

                // this is done multiple unnecessary times
                // because the ScreenBreakPointProperty is evaluated on each breakpoint.
                // is it worth to group all the breakpoints in a single evaluation?

                // The good thing is that the update is done only once, because the ScreenBreakPointProperty
                // is updated only once.
                _windowHandler = (sender, args) =>
                {
                    var current = (Breakpoint)v.GetValue(Has.ScreenBreakPointProperty);
                    var breakpoint = GetBreakpoint(v);

                    if (current == breakpoint) return;

                    v.SetValue(Has.ScreenBreakPointProperty, breakpoint);
                };

                v.Window.SizeChanged += _windowHandler;

                // initial value.
                v.SetValue(Has.ScreenBreakPointProperty, GetBreakpoint(v));

                return [new(v, [Has.ScreenBreakPointProperty.PropertyName])];
            }
        };
    }

    public OperatorKind Operator { get; set; } = OperatorKind.GreaterThanOrEquals;

    public override void Dispose()
    {
        if (_element is not null)
            _element.Window.SizeChanged -= _windowHandler;

        _element = null;

        base.Dispose();
    }

    protected bool Compare(VisualElement visualElement, Breakpoint breakpoint)
    {
        var comparer = s_comparers[Operator];
        var visualElementBreakpoint = (Breakpoint)visualElement.GetValue(Has.ScreenBreakPointProperty);

        return comparer(visualElementBreakpoint, breakpoint);
    }

    private static Breakpoint GetBreakpoint(VisualElement visualElement)
    {
        var w = visualElement.Window.Width;

        var maxBreakpoint = Breakpoint.Xs;

        if (w > (int)Breakpoint.Sm) maxBreakpoint = Breakpoint.Sm;
        if (w > (int)Breakpoint.Md) maxBreakpoint = Breakpoint.Md;
        if (w > (int)Breakpoint.Lg) maxBreakpoint = Breakpoint.Lg;
        if (w > (int)Breakpoint.Xl) maxBreakpoint = Breakpoint.Xl;
        if (w > (int)Breakpoint.Xxl) maxBreakpoint = Breakpoint.Xxl;

        return maxBreakpoint;
    }

    public enum OperatorKind
    {
        Equals,
        GreaterThanOrEquals,
        GreaterThan,
        LessThanOrEquals,
        LessThan
    }
}
