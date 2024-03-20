namespace Manuela;

public class On
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ResponsiveStyleProperty = BindableProperty.CreateAttached(
        "ResponsiveStyle", typeof(ResponsiveStyle), typeof(On), null);

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

    private static BindableProperty.BindingPropertyChangedDelegate GetBreakpointStyleChangedDelegate(BreakPoint p)
    {
        return (BindableObject bindable, object? oldValue, object? newValue) =>
        {
            if (newValue is null) return;
            var newStyle = (ManuelaStyle)newValue;

            var styleSet = (ResponsiveStyle?)bindable.GetValue(ResponsiveStyleProperty);
            styleSet ??= new();

            if (!styleSet.IsInitialized)
            {
                styleSet.Initialize(bindable);

                if (bindable is VisualElement ve && ve.Window is not null)
                {
                    // when the window is already set, we need to apply the style now.
                    //styleSet.Apply(ResponsiveStyle.GetBreakpoint(ve.Width));
                }
            }

            switch (p)
            {
                case BreakPoint.all: styleSet.All = newStyle; break;
                case BreakPoint.sm: styleSet.Sm = newStyle; break;
                case BreakPoint.md: styleSet.Md = newStyle; break;
                case BreakPoint.lg: styleSet.Lg = newStyle; break;
                case BreakPoint.xl: styleSet.Xl = newStyle; break;
                case BreakPoint.xxl: styleSet.Xxl = newStyle; break;
                default: break;
            }

            bindable.SetValue(ResponsiveStyleProperty, styleSet);
        };
    }
}
