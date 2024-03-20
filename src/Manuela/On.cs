namespace Manuela;

public class On
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ResponsiveStyleProperty = BindableProperty.CreateAttached(
        "ResponsiveStyle", typeof(ResponsiveStyle), typeof(On), null);

    public static BindableProperty AllProperty = BindableProperty.CreateAttached(
        "All", typeof(Set), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.all));
    public static BindableProperty SmProperty = BindableProperty.CreateAttached(
        "Sm", typeof(Set), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.sm));
    public static BindableProperty MdProperty = BindableProperty.CreateAttached(
        "Md", typeof(Set), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.md));
    public static BindableProperty LgProperty = BindableProperty.CreateAttached(
        "Lg", typeof(Set), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.lg));
    public static BindableProperty XlProperty = BindableProperty.CreateAttached(
        "Xl", typeof(Set), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.xl));
    public static BindableProperty XxlProperty = BindableProperty.CreateAttached(
        "Xxl", typeof(Set), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.xxl));
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static Set GetAll(BindableObject view) => (Set)view.GetValue(AllProperty);
    public static void SetAll(BindableObject view, Set value) => view.SetValue(AllProperty, value);

    public static Set GetSm(BindableObject view) => (Set)view.GetValue(SmProperty);
    public static void SetSm(BindableObject view, Set value) => view.SetValue(SmProperty, value);

    public static Set GetMd(BindableObject view) => (Set)view.GetValue(MdProperty);
    public static void SetMd(BindableObject view, Set value) => view.SetValue(MdProperty, value);

    public static Set GetLg(BindableObject view) => (Set)view.GetValue(LgProperty);
    public static void SetLg(BindableObject view, Set value) => view.SetValue(LgProperty, value);

    public static Set GetXl(BindableObject view) => (Set)view.GetValue(XlProperty);
    public static void SetXl(BindableObject view, Set value) => view.SetValue(XlProperty, value);

    public static Set GetXxl(BindableObject view) => (Set)view.GetValue(XxlProperty);
    public static void SetXxl(BindableObject view, Set value) => view.SetValue(XxlProperty, value);

    private static BindableProperty.BindingPropertyChangedDelegate GetBreakpointStyleChangedDelegate(BreakPoint p)
    {
        return (BindableObject bindable, object? oldValue, object? newValue) =>
        {
            if (newValue is null) return;
            var newStyle = ((Set)newValue).Setters;

            var styleSet = (ResponsiveStyle?)bindable.GetValue(ResponsiveStyleProperty);
            styleSet ??= new();

            if (!styleSet.IsInitialized)
            {
                styleSet.Initialize(bindable);
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
