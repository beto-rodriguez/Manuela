namespace Manuela;

public class On
{
    public static Dictionary<int, int> Screens { get; set; } = new()
    {
        { (int)BreakPoint.sm, 640 },
        { (int)BreakPoint.md, 768 },
        { (int)BreakPoint.lg, 1024 },
        { (int)BreakPoint.xl, 1280 },
        { (int)BreakPoint.xxl, 1536 }
    };

    #region Attached Properties things...

#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ManuelaStyleSetProperty = BindableProperty.CreateAttached(
        "ManuelaStyleSet", typeof(ManuelaStyleSet), typeof(On), new ManuelaStyleSet());

    public static BindableProperty AllProperty = BindableProperty.CreateAttached(
        "All", typeof(ManuelaStyle), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.all));
    public static BindableProperty SmProperty = BindableProperty.CreateAttached(
        "Sm", typeof(ManuelaStyle), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.sm));
    public static BindableProperty MdProperty = BindableProperty.CreateAttached(
        "Md", typeof(ManuelaStyle), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.md));
    public static BindableProperty LgProperty = BindableProperty.CreateAttached(
        "Lg", typeof(ManuelaStyle), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.lg));
    public static BindableProperty XlProperty = BindableProperty.CreateAttached(
        "Xl", typeof(ManuelaStyle), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.xl));
    public static BindableProperty XxlProperty = BindableProperty.CreateAttached(
        "Xxl", typeof(ManuelaStyle), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.xxl));
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static ManuelaStyle GetAll(BindableObject view) => (ManuelaStyle)view.GetValue(AllProperty);
    public static void SetAll(BindableObject view, ManuelaStyle value) => view.SetValue(AllProperty, value);

    public static ManuelaStyle GetSm(BindableObject view) => (ManuelaStyle)view.GetValue(SmProperty);
    public static void SetSm(BindableObject view, ManuelaStyle value) => view.SetValue(SmProperty, value);

    public static ManuelaStyle GetMd(BindableObject view) => (ManuelaStyle)view.GetValue(MdProperty);
    public static void SetMd(BindableObject view, ManuelaStyle value) => view.SetValue(MdProperty, value);

    public static ManuelaStyle GetLg(BindableObject view) => (ManuelaStyle)view.GetValue(LgProperty);
    public static void SetLg(BindableObject view, ManuelaStyle value) => view.SetValue(LgProperty, value);

    public static ManuelaStyle GetXl(BindableObject view) => (ManuelaStyle)view.GetValue(XlProperty);
    public static void SetXl(BindableObject view, ManuelaStyle value) => view.SetValue(XlProperty, value);

    public static ManuelaStyle GetXxl(BindableObject view) => (ManuelaStyle)view.GetValue(XxlProperty);
    public static void SetXxl(BindableObject view, ManuelaStyle value) => view.SetValue(XxlProperty, value);

    #endregion

    public static BreakPoint GetBreakpoint(double width)
    {
        var breakPoint = BreakPoint.all;

        if (width >= Screens[1]) breakPoint = BreakPoint.md;
        if (width >= Screens[2]) breakPoint = BreakPoint.lg;
        if (width >= Screens[3]) breakPoint = BreakPoint.xl;
        if (width >= Screens[4]) breakPoint = BreakPoint.xxl;

        return breakPoint;
    }

    private static BindableProperty.BindingPropertyChangedDelegate GetBreakpointStyleChangedDelegate(BreakPoint p)
    {
        return (BindableObject bindable, object? oldValue, object? newValue) =>
        {
            if (newValue is null) return;
            var newStyle = (ManuelaStyle)newValue;

            var styleSet = (ManuelaStyleSet)bindable.GetValue(ManuelaStyleSetProperty);

            if (!styleSet.IsInitialized)
            {
                styleSet.BindableObject = bindable;
                bindable.PropertyChanging += OnBindablePropertyChanging;
                bindable.PropertyChanged += OnBindablePropertyChanged;
                styleSet.IsInitialized = true;
            }

            switch (p)
            {
                case BreakPoint.all: styleSet.All = newStyle; break;
                case BreakPoint.sm: styleSet.Sm = newStyle; break;
                case BreakPoint.md: styleSet.Md = newStyle; break;
                case BreakPoint.lg: styleSet.Lg = newStyle; break;
                case BreakPoint.xl: styleSet.Xl = newStyle; break;
                case BreakPoint.xxl: styleSet.Xxl = newStyle; break;
                case BreakPoint.unknown: default: break;
            }

            bindable.SetValue(ManuelaStyleSetProperty, styleSet);
        };
    }

    private static void OnBindablePropertyChanging(object sender, PropertyChangingEventArgs e)
    {
        if (e.PropertyName != nameof(VisualElement.Window) || sender is not VisualElement ve) return;

        if (ve.Window is not null)
        {
            ve.Window.SizeChanged -= OnWindowSizeChanged;
        }
    }

    private static void OnBindablePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(VisualElement.Window) || sender is not VisualElement ve) return;

        if (ve.Window is not null)
        {
            ve.Window.SizeChanged += OnWindowSizeChanged;
        }
    }

    private static void OnWindowSizeChanged(object? sender, EventArgs e)
    {
        var window = (Window?)sender;
        if (window is null) return;

        var p = GetBreakpoint(window.Width);
        var styleSet = (ManuelaStyleSet)window.GetValue(ManuelaStyleSetProperty);

        if (styleSet.ActiveBreakPoint == p || styleSet.BindableObject is null) return;

        styleSet.ActiveBreakPoint = p;
        styleSet.Apply(p, styleSet.BindableObject);
    }
}
