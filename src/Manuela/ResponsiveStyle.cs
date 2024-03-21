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
    public BindableObject? BindableObject { get; private set; }
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

        var bindable = BindableObject;
        if (bindable is null) return;

        var hashSet = new HashSet<ManuelaProperty>();

        var query = (All?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Sm?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Md?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Lg?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Xl?.Keys ?? Enumerable.Empty<ManuelaProperty>())
            .Concat(Xxl?.Keys ?? Enumerable.Empty<ManuelaProperty>());

        foreach (var property in query)
        {
            if (hashSet.Contains(property)) continue;

            var bindableProperty = ManuelaThings.GetBindableProperty(BindableObject, property);

            if (bindableProperty is null)
            {
#if DEBUG
                Trace.WriteLine($"Property {property} is not supported on {bindable.GetType().Name}");
#endif
                continue;
            }

            var value = Get(property, p);

            if (value == Unset)
            {
                bindable.ClearValue(bindableProperty);
            }
            else
            {
                bindable.SetValue(bindableProperty, value);
            }

            _ = hashSet.Add(property);
        }

        ActiveBreakPoint = p;
    }

    public void Initialize(BindableObject bindable)
    {
        BindableObject = bindable;
        bindable.PropertyChanging += OnBindablePropertyChanging;
        bindable.PropertyChanged += OnBindablePropertyChanged;
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
        if (window is null || BindableObject is null) return;

        var p = GetBreakpoint(window.Width);
        var styleSet = (ResponsiveStyle)BindableObject.GetValue(On.ResponsiveStyleProperty);

        if (styleSet.ActiveBreakPoint == p || styleSet.BindableObject is null) return;

        styleSet.Apply(p);
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

    private object? Get(ManuelaProperty property, BreakPoint p)
    {
        var value = Unset;

        if (All?.TryGetValue(property, out var all) ?? false) value = all;

        if (p >= BreakPoint.sm && (Sm?.TryGetValue(property, out var sm) ?? false)) value = sm;
        if (p >= BreakPoint.md && (Md?.TryGetValue(property, out var md) ?? false)) value = md;
        if (p >= BreakPoint.lg && (Lg?.TryGetValue(property, out var lg) ?? false)) value = lg;
        if (p >= BreakPoint.xl && (Xl?.TryGetValue(property, out var xl) ?? false)) value = xl;
        if (p >= BreakPoint.xxl && (Xxl?.TryGetValue(property, out var xxl) ?? false)) value = xxl;

        if (value == Unset || BindableObject is null) return value;

        return ManuelaThings.TryConvert(BindableObject, property, value);
    }
}
