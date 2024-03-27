namespace Manuela.Styling.ConditionalStyles.Screen;

public class OnScreenSize : ConditionalStyle
{
    private static readonly Dictionary<Breakpoint, double> s_screenSizes = new()
    {
        { Breakpoint.Xs, 0 },
        { Breakpoint.Sm, 640 },
        { Breakpoint.Md, 768 },
        { Breakpoint.Lg, 1024 },
        { Breakpoint.Xl, 1280 },
        { Breakpoint.Xxl, 1536 }
    };

    public OnScreenSize(Breakpoint targetBreakpoint)
    {
        Condition = new(visualElement => (Breakpoint)visualElement.GetValue(Has.ScreenBreakPointProperty) >= targetBreakpoint)
        {
            Triggers = v =>
            {
                // this is done multiple unnecessary times
                // because the ScreenBreakPointProperty is evaluated on each breakpoint.
                // is it worth to group all the breakpoints in a single evaluation?

                // The good thing is that the update is done only once, because the ScreenBreakPointProperty
                // is updated only once.

                v.Window.SizeChanged += (sender, args) =>
                {
                    var current = (Breakpoint)v.GetValue(Has.ScreenBreakPointProperty);
                    var breakpoint = GetBreakpoint(v);

                    if (current == breakpoint) return;

                    v.SetValue(Has.ScreenBreakPointProperty, breakpoint);
                };

                // initial value.
                v.SetValue(Has.ScreenBreakPointProperty, GetBreakpoint(v));

                return [new(v, [Has.ScreenBreakPointProperty.PropertyName])];
            }
        };
    }

    private Breakpoint GetBreakpoint(VisualElement visualElement)
    {
        var w = visualElement.Window.Width;

        var maxBreakpoint = Breakpoint.Xs;

        if (w > s_screenSizes[Breakpoint.Sm]) maxBreakpoint = Breakpoint.Sm;
        if (w > s_screenSizes[Breakpoint.Md]) maxBreakpoint = Breakpoint.Md;
        if (w > s_screenSizes[Breakpoint.Lg]) maxBreakpoint = Breakpoint.Lg;
        if (w > s_screenSizes[Breakpoint.Xl]) maxBreakpoint = Breakpoint.Xl;
        if (w > s_screenSizes[Breakpoint.Xxl]) maxBreakpoint = Breakpoint.Xxl;

        return maxBreakpoint;
    }
}
