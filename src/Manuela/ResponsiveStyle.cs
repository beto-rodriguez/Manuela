using System.Diagnostics;
namespace Manuela;

public class ResponsiveStyle
{
    public static Dictionary<int, int> Screens { get; set; } = new()
    {
        { (int)BreakPoint.sm, 640 },
        { (int)BreakPoint.md, 768 },
        { (int)BreakPoint.lg, 1024 },
        { (int)BreakPoint.xl, 1280 },
        { (int)BreakPoint.xxl, 1536 }
    };

    public static object Unset { get; } = new();
    public BreakPoint ActiveBreakPoint { get; private set; }
    public VisualElement? VisualElement { get; private set; }

    // Based on Note #1
    // in this case initialization could be shared between instances
    // because we are subscribing to the VisualElement.Window.SizeChanged event
    // in theory the window instance is the same for both?
    public bool IsInitialized { get; private set; }
    public ManuelaStyle? All { get; set; }
    public ManuelaStyle? Sm { get; set; }
    public ManuelaStyle? Md { get; set; }
    public ManuelaStyle? Lg { get; set; }
    public ManuelaStyle? Xl { get; set; }
    public ManuelaStyle? Xxl { get; set; }

    public void Apply(BreakPoint p)
    {
        if (!IsInitialized) return;

        var visual = VisualElement;
        if (visual is null) return;

        var hashSet = new HashSet<ManuelaProperty>();

        var query = (All?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Sm?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Md?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Lg?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Xl?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Xxl?.Keys ?? Enumerable.Empty<ManuelaProperty>());

        var states = (StatesCollection?)visual.GetValue(On.ConditionalStatesProperty);

        ActiveBreakPoint = p;

        foreach (var property in query)
        {
            // if the property was already evaluated, skip it
            if (hashSet.Contains(property)) continue;

            var bindableProperty = ManuelaThings.GetBindableProperty(VisualElement, property);

            if (bindableProperty is null)
            {
#if DEBUG
                Trace.WriteLine($"Property {property} is not supported on {visual.GetType().Name}");
#endif
                continue;
            }

            var stateApplied = states?.ApplyFirstPropertyMet(visual, property, bindableProperty) ?? false;

            if (!stateApplied)
                ApplyProperty(visual, property, bindableProperty);

            _ = hashSet.Add(property);
        }
    }

    public void ApplyProperty(VisualElement visual, ManuelaProperty property, BindableProperty bindableProperty)
    {
        var value = Get(property);

        if (value == Unset)
        {
            visual.ClearValue(bindableProperty);
        }
        else
        {
            visual.SetValue(bindableProperty, value);
        }
    }

    public void Initialize(VisualElement visual)
    {
        VisualElement = visual;
        visual.PropertyChanging += OnBindablePropertyChanging;
        visual.PropertyChanged += OnBindablePropertyChanged;
        IsInitialized = true;
    }

    private void OnBindablePropertyChanging(object sender, PropertyChangingEventArgs e)
    {
        if (e.PropertyName != nameof(VisualElement.Window) || sender is not VisualElement ve) return;

        if (ve.Window is not null)
        {
            ve.Window.SizeChanged -= OnWindowSizeChanged;
        }
    }

    private void OnBindablePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(VisualElement.Window) || sender is not VisualElement ve) return;

        if (ve.Window is not null)
        {
            ve.Window.SizeChanged += OnWindowSizeChanged;
        }
    }

    private void OnWindowSizeChanged(object? sender, EventArgs e)
    {
        var window = (Window?)sender;
        if (window is null || VisualElement is null) return;

        var p = GetBreakpoint(window.Width);
        var responsiveStyle = (ResponsiveStyle)VisualElement.GetValue(On.ResponsiveStyleProperty);

        if (responsiveStyle.ActiveBreakPoint == p || responsiveStyle.VisualElement is null) return;

        responsiveStyle.Apply(p);
    }

    public static BreakPoint GetBreakpoint(double width)
    {
        var breakPoint = BreakPoint.all;

        if (width >= Screens[1]) breakPoint = BreakPoint.md;
        if (width >= Screens[2]) breakPoint = BreakPoint.lg;
        if (width >= Screens[3]) breakPoint = BreakPoint.xl;
        if (width >= Screens[4]) breakPoint = BreakPoint.xxl;

        return breakPoint;
    }

    private object? Get(ManuelaProperty property)
    {
        var p = ActiveBreakPoint;

        var value = Unset;

        if (All?.TryGetValue(property, out var all) ?? false) value = all;

        if (p >= BreakPoint.sm && (Sm?.TryGetValue(property, out var sm) ?? false)) value = sm;
        if (p >= BreakPoint.md && (Md?.TryGetValue(property, out var md) ?? false)) value = md;
        if (p >= BreakPoint.lg && (Lg?.TryGetValue(property, out var lg) ?? false)) value = lg;
        if (p >= BreakPoint.xl && (Xl?.TryGetValue(property, out var xl) ?? false)) value = xl;
        if (p >= BreakPoint.xxl && (Xxl?.TryGetValue(property, out var xxl) ?? false)) value = xxl;

        if (value == Unset || VisualElement is null) return value;

        return ManuelaThings.TryConvert(VisualElement, property, value);
    }
}
