// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling;
using Manuela.Styling.ConditionalStyles;
using Manuela.Styling.ConditionalStyles.Screen;

namespace Manuela;

public class OnScreenSize : ConditionalStyle
{
    private VisualElement? _element;
    private Window? _window;
    private EventHandler? _windowHandler;
    private static readonly Breakpoint[] s_breakpoints =
        [Breakpoint.Xs, Breakpoint.Sm, Breakpoint.Md, Breakpoint.Lg, Breakpoint.Xl, Breakpoint.Xxl];

    public OnScreenSize()
    {
        // true... becase every time the breakpoint changes, we will build a new set of setters (GetSetters()) and apply them.
        Condition = new(visualElement => true);
    }

    private readonly Dictionary<Breakpoint, ManuelaSettersDictionary?> _setters = [];
    private readonly Dictionary<Breakpoint, Breakpoint> _maxBreakpoint = [];

    public ManuelaSettersDictionary? Xs { get => _setters[Breakpoint.Xs]; set => _setters[Breakpoint.Xs] = value; }
    public ManuelaSettersDictionary? Sm { get => _setters[Breakpoint.Sm]; set => _setters[Breakpoint.Sm] = value; }
    public ManuelaSettersDictionary? Md { get => _setters[Breakpoint.Md]; set => _setters[Breakpoint.Md] = value; }
    public ManuelaSettersDictionary? Lg { get => _setters[Breakpoint.Lg]; set => _setters[Breakpoint.Lg] = value; }
    public ManuelaSettersDictionary? Xl { get => _setters[Breakpoint.Xl]; set => _setters[Breakpoint.Xl] = value; }
    public ManuelaSettersDictionary? Xxl { get => _setters[Breakpoint.Xxl]; set => _setters[Breakpoint.Xxl] = value; }

    public Breakpoint XsMaxBreakpoint { get => _maxBreakpoint[Breakpoint.Xs]; set => _maxBreakpoint[Breakpoint.Xs] = value; }
    public Breakpoint SmMaxBreakpoint { get => _maxBreakpoint[Breakpoint.Sm]; set => _maxBreakpoint[Breakpoint.Sm] = value; }
    public Breakpoint MdMaxBreakpoint { get => _maxBreakpoint[Breakpoint.Md]; set => _maxBreakpoint[Breakpoint.Md] = value; }
    public Breakpoint LgMaxBreakpoint { get => _maxBreakpoint[Breakpoint.Lg]; set => _maxBreakpoint[Breakpoint.Lg] = value; }
    public Breakpoint XlMaxBreakpoint { get => _maxBreakpoint[Breakpoint.Xl]; set => _maxBreakpoint[Breakpoint.Xl] = value; }

    public override ManuelaSettersDictionary? GetSetters()
    {
        var currentBp = GetBreakpoint();

        var mergedSetters = new ManuelaSettersDictionary();

        foreach (var bp in s_breakpoints)
        {
            if (!_maxBreakpoint.TryGetValue(bp, out var maxBp)) maxBp = Breakpoint.Xxl;

            var isValid = currentBp >= bp && currentBp <= maxBp;
            if (!isValid) continue;

            if (!_setters.TryGetValue(bp, out var bpSetters) || bpSetters is null) continue;

            foreach (var setter in bpSetters)
                mergedSetters[setter.Key] = setter.Value;
        }

        return mergedSetters;
    }

    public override void Dispose(VisualElement visualElement)
    {
        if (_window is not null)
            _window.SizeChanged -= _windowHandler;

        _element = null;
        _window = null;

        base.Dispose(visualElement);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        _element = visualElement;
        _window = visualElement.Window;

        _windowHandler = (sender, args) =>
        {
            var current = (Breakpoint)visualElement.GetValue(Has.ScreenBreakPointProperty);
            var breakpoint = GetBreakpoint();

            if (current == breakpoint) return;
            visualElement.SetValue(Has.ScreenBreakPointProperty, breakpoint);
        };

        _window.SizeChanged += _windowHandler;

        // initial value.
        visualElement.SetValue(Has.ScreenBreakPointProperty, GetBreakpoint());

        Condition.Triggers = [new(visualElement, [Has.ScreenBreakPointProperty.PropertyName])];
    }

    private Breakpoint GetBreakpoint()
    {
        return GetBreakpoint(_element, _window);
    }

    public static Breakpoint GetBreakpoint(VisualElement? element, Window? window)
    {
        if (element is null || window is null) return Breakpoint.Xs;

        var w = window.Width;
        var maxBreakpoint = Breakpoint.Xs;

        if (w > (int)Breakpoint.Sm) maxBreakpoint = Breakpoint.Sm;
        if (w > (int)Breakpoint.Md) maxBreakpoint = Breakpoint.Md;
        if (w > (int)Breakpoint.Lg) maxBreakpoint = Breakpoint.Lg;
        if (w > (int)Breakpoint.Xl) maxBreakpoint = Breakpoint.Xl;
        if (w > (int)Breakpoint.Xxl) maxBreakpoint = Breakpoint.Xxl;

        return maxBreakpoint;
    }
}
