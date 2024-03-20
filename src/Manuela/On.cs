
using System.Diagnostics;

namespace Manuela;

public class On
{
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

    private static BindableProperty.BindingPropertyChangedDelegate GetBreakpointStyleChangedDelegate(BreakPoint p)
    {
        return (BindableObject bindable, object? oldValue, object? newValue) =>
        {
            if (newValue is null) return;
            var newStyle = (ManuelaStyle)newValue;

            var styleSet = (ManuelaStyleSet)bindable.GetValue(ManuelaStyleSetProperty);

            if (!styleSet.IsInitialized)
            {
                bindable.PropertyChanging += OnBindablePropertyChanging;
                bindable.PropertyChanged += OnBindablePropertyChanged;
                styleSet = styleSet.SetInitialized(true);
            }

            styleSet = p switch
            {
                BreakPoint.all => styleSet.SetAll(newStyle),
                BreakPoint.sm => styleSet.SetSm(newStyle),
                BreakPoint.md => styleSet.SetMd(newStyle),
                BreakPoint.lg => styleSet.SetLg(newStyle),
                BreakPoint.xl => styleSet.SetXl(newStyle),
                BreakPoint.xxl => styleSet.SetXxl(newStyle),
                _ => throw new ArgumentOutOfRangeException(nameof(p), p, null),
            };

            bindable.SetValue(ManuelaStyleSetProperty, styleSet);
        };
    }

    private static void OnBindablePropertyChanging(object sender, PropertyChangingEventArgs e)
    {
        if (e.PropertyName != nameof(VisualElement.Window)) return;
        var a = ((VisualElement)sender).Window;
        var b = 1;
    }

    private static void OnBindablePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        Console.WriteLine("Property changed" + e.PropertyName);
        if (e.PropertyName != nameof(VisualElement.Window)) return;
        var window = ((VisualElement)sender).Window;
        //window.SizeChanged += OnWindowSizeChanged;
        var b = 1;
    }

    private static void OnWindowSizeChanged(object? sender, EventArgs e)
    {
        Trace.WriteLine("Window size changed");
        Apply();
    }

    private static void Apply()
    {

    }
}

