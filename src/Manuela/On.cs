namespace Manuela;

public class Condition
{
    public Func<VisualElement, bool> Predicate { get; set; }
    public Func<VisualElement, ConditionUpdateTrigger[]> Triggers { get; set; }
}

public class ConditionUpdateTrigger(BindableObject bindable, HashSet<string> properties)
{
    public BindableObject Target { get; } = bindable;
    public HashSet<string> Properties { get; } = properties;
}

public class On
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ResponsiveStyleProperty = BindableProperty.CreateAttached(
        "ResponsiveStyle", typeof(ResponsiveStyle), typeof(On), null);

    public static BindableProperty AllScreensProperty = BindableProperty.CreateAttached(
        "AllScreens", typeof(Style), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.all));
    public static BindableProperty SmProperty = BindableProperty.CreateAttached(
        "Sm", typeof(Style), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.sm));
    public static BindableProperty MdProperty = BindableProperty.CreateAttached(
        "Md", typeof(Style), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.md));
    public static BindableProperty LgProperty = BindableProperty.CreateAttached(
        "Lg", typeof(Style), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.lg));
    public static BindableProperty XlProperty = BindableProperty.CreateAttached(
        "Xl", typeof(Style), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.xl));
    public static BindableProperty XxlProperty = BindableProperty.CreateAttached(
        "Xxl", typeof(Style), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.xxl));

    public static BindableProperty ConditionalStatesProperty = BindableProperty.CreateAttached(
        "ConditionalStates", typeof(StatesCollection), typeof(On), null, propertyChanged: OnStateChanged);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static Style GetAllScreens(BindableObject view) => (Style)view.GetValue(AllScreensProperty);
    public static void SetAllScreens(BindableObject view, Style value) => view.SetValue(AllScreensProperty, value);

    public static Style GetSm(BindableObject view) => (Style)view.GetValue(SmProperty);
    public static void SetSm(BindableObject view, Style value) => view.SetValue(SmProperty, value);

    public static Style GetMd(BindableObject view) => (Style)view.GetValue(MdProperty);
    public static void SetMd(BindableObject view, Style value) => view.SetValue(MdProperty, value);

    public static Style GetLg(BindableObject view) => (Style)view.GetValue(LgProperty);
    public static void SetLg(BindableObject view, Style value) => view.SetValue(LgProperty, value);

    public static Style GetXl(BindableObject view) => (Style)view.GetValue(XlProperty);
    public static void SetXl(BindableObject view, Style value) => view.SetValue(XlProperty, value);

    public static Style GetXxl(BindableObject view) => (Style)view.GetValue(XxlProperty);
    public static void SetXxl(BindableObject view, Style value) => view.SetValue(XxlProperty, value);

    public static StatesCollection GetConditionalStates(BindableObject view) => (StatesCollection)view.GetValue(ConditionalStatesProperty);
    public static void SetConditionalStates(BindableObject view, StatesCollection value) => view.SetValue(ConditionalStatesProperty, value);

    public static void OnStateChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (newValue is null || bindable is not VisualElement ve) return;

        var statesCollection = (StatesCollection?)newValue ?? [];

        foreach (var conditionalSet in statesCollection)
        {
            conditionalSet.PropertyChanged += (sender, e) =>
            {
                if (conditionalSet.Condition is null || conditionalSet.IsInitialized.Contains(bindable)) return;

                var a = ve.BindingContext;
                conditionalSet.Initialize(ve);
            };

            ve.AddLogicalChild(conditionalSet);
        }
    }

    private static BindableProperty.BindingPropertyChangedDelegate GetBreakpointStyleChangedDelegate(BreakPoint p)
    {
        return (BindableObject bindable, object? oldValue, object? newValue) =>
        {
            if (newValue is null || bindable is not VisualElement ve) return;
            var newStyle = ((Style)newValue).Setters;

            var styleSet = (ResponsiveStyle?)bindable.GetValue(ResponsiveStyleProperty);
            styleSet ??= new();

            if (!styleSet.IsInitialized)
            {
                styleSet.Initialize(ve);
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
