using System.Diagnostics;
using Manuela.Theming;

namespace Manuela;

public class SetIf : Element
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty ConditionProperty = BindableProperty.Create(
        nameof(Condition), typeof(Condition), typeof(StatesCollection), null);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public Set Setters { get; set; }

    public Condition? Condition
    {
        get => (Condition)GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    // Note #1
    // initialization must be per bindable.
    // to avoid a possible issue when a resource is shared using x:StaticResource.
    public HashSet<VisualElement> IsInitialized { get; } = [];

    public void Initialize(VisualElement bindable)
    {
        if (bindable is VisualElement ve)
        {
            var triggers = Condition?.Triggers(ve) ?? [];

            foreach (var trigger in triggers)
            {
                trigger.Target.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName is null || !trigger.Properties.Contains(e.PropertyName))
                        return;

                    // at this point, the target property changed and the condition was met
                    Apply(bindable);
                };
            }
        }

        _ = IsInitialized.Add(bindable);
        Apply(bindable);
    }

    public void Apply(BindableObject? bindable)
    {
        if (bindable is null || !IsInitialized.Contains(bindable) || bindable is not VisualElement ve) return;

        foreach (var property in Setters.Setters.Keys)
        {
            var bindableProperty = ManuelaThings.GetBindableProperty(bindable, property);

            if (bindableProperty is null)
            {
#if DEBUG
                Trace.WriteLine($"Property {property} is not supported on {bindable.GetType().Name}");
#endif
                continue;
            }

            if (Condition?.Predicate(ve) == true)
            {
                if (!Setters.Setters.TryGetValue(property, out var value)) continue;
                value = ManuelaThings.TryConvert(bindable, property, value);
                bindable.SetValue(bindableProperty, value);
            }
            else
            {
                var responsiveStyle = (ResponsiveStyle?)bindable.GetValue(On.ResponsiveStyleProperty);
                if (responsiveStyle is null)
                {
                    bindable.ClearValue(bindableProperty);
                }
                else
                {
                    responsiveStyle.Apply(responsiveStyle.ActiveBreakPoint);
                }
            }
        }
    }
}

public class StatesCollection : List<SetIf>
{ }

public static class ManuelaThings
{
    private static readonly Dictionary<ManuelaProperty, Func<BindableObject, BindableProperty?>> s_propertiesMap = new()
    {
        { ManuelaProperty.Background, bindable => VisualElement.BackgroundProperty },
        { ManuelaProperty.Margin, bindable => View.MarginProperty },
        { ManuelaProperty.Padding, bindable =>
            {
                if (bindable is Border) return Border.PaddingProperty;
                if (bindable is Button) return Button.PaddingProperty;
                if (bindable is Microsoft.Maui.Controls.Compatibility.Layout) return Microsoft.Maui.Controls.Compatibility.Layout.PaddingProperty;
                if (bindable is ImageButton) return ImageButton.PaddingProperty;
                if (bindable is Label) return Label.PaddingProperty;
                if (bindable is Layout) return Layout.PaddingProperty;
                if (bindable is Page) return Page.PaddingProperty;
                return null;
            }
        },
        { ManuelaProperty.BorderColor, bindable =>
            {
                if (bindable is Border) return Border.StrokeProperty;
                if (bindable is Button) return Button.BorderColorProperty;
                if (bindable is Frame) return Frame.BorderColorProperty;
                return null;
            }
        },
        { ManuelaProperty.BorderThickness, bindable =>
            {
                if (bindable is Border) return Border.StrokeThicknessProperty;
                if (bindable is Button) return Button.BorderWidthProperty;
                return null;
            }
        },
        { ManuelaProperty.BorderRadius, bindable =>
            {
                if (bindable is Border) return Border.StrokeShapeProperty;
                if (bindable is Button) return Button.CornerRadiusProperty;
                if (bindable is Frame) return Frame.CornerRadiusProperty;
                return null;
            }
        },
        { ManuelaProperty.Shadow, bindable => VisualElement.ShadowProperty },
        { ManuelaProperty.TextSize, bindable =>
            {
                if (bindable is Button) return Button.FontSizeProperty;
                if (bindable is DatePicker) return DatePicker.FontSizeProperty;
                if (bindable is Editor) return Editor.FontSizeProperty;
                if (bindable is Entry) return Entry.FontSizeProperty;
                if (bindable is SearchBar) return SearchBar.FontSizeProperty;
                if (bindable is Label) return Label.FontSizeProperty;
                if (bindable is Picker) return Picker.FontSizeProperty;
                if (bindable is RadioButton) return RadioButton.FontSizeProperty;
                if (bindable is SearchHandler) return SearchHandler.FontSizeProperty;
                if (bindable is Span) return Span.FontSizeProperty;
                if (bindable is TimePicker) return TimePicker.FontSizeProperty;
                //if (bindable is TextInput) return TextInput.FontSizeProperty;
                //if (bindable is TextAreaInput) return TextAreaInput.FontSizeProperty;
                //if (bindable is DatePickerInput) return DatePickerInput.FontSizeProperty;
                //if (bindable is PickerInput) return PickerInput.FontSizeProperty;
                return null;
            }
        },
        { ManuelaProperty.LineHeight, bindable =>
            {
                if (bindable is Label) return Label.LineHeightProperty;
                if (bindable is Span) return Span.LineHeightProperty;
                return null;
            }
        },
        { ManuelaProperty.FontAttributes, bindable =>
            {
                if (bindable is Button) return Button.FontAttributesProperty;
                if (bindable is DatePicker) return DatePicker.FontAttributesProperty;
                if (bindable is Editor) return Editor.FontAttributesProperty;
                if (bindable is Entry) return Entry.FontAttributesProperty;
                if (bindable is SearchBar) return SearchBar.FontAttributesProperty;
                if (bindable is Label) return Label.FontAttributesProperty;
                if (bindable is Picker) return Picker.FontAttributesProperty;
                if (bindable is RadioButton) return RadioButton.FontAttributesProperty;
                if (bindable is SearchHandler) return SearchHandler.FontAttributesProperty;
                if (bindable is Span) return Span.FontAttributesProperty;
                if (bindable is TimePicker) return TimePicker.FontAttributesProperty;
                //if (bindable is TextInput) return TextInput.FontAttributesProperty;
                //if (bindable is TextAreaInput) return TextAreaInput.FontAttributesProperty;
                //if (bindable is DatePickerInput) return DatePickerInput.FontAttributesProperty;
                //if (bindable is PickerInput) return PickerInput.FontAttributesProperty;
                return null;
            }
        },
        { ManuelaProperty.VerticalTextAlign, bindable =>
            {
                if (bindable is Label) return Label.VerticalTextAlignmentProperty;
                return null;
            }
        },
        { ManuelaProperty.HorizontalTextAlign, bindable =>
            {
                if (bindable is Label) return Label.HorizontalTextAlignmentProperty;
                return null;
            }
        },
        { ManuelaProperty.TextColor, bindable =>
            {
                if (bindable is Button) return Button.TextColorProperty;
                if (bindable is DatePicker) return DatePicker.TextColorProperty;
                if (bindable is Editor) return Editor.TextColorProperty;
                if (bindable is Entry) return Entry.TextColorProperty;
                if (bindable is SearchBar) return SearchBar.TextColorProperty;
                if (bindable is Label) return Label.TextColorProperty;
                if (bindable is Picker) return Picker.TextColorProperty;
                if (bindable is SearchHandler) return SearchHandler.TextColorProperty;
                if (bindable is Span) return Span.TextColorProperty;
                if (bindable is RadioButton) return RadioButton.TextColorProperty;
                if (bindable is TimePicker) return TimePicker.TextColorProperty;
                //if (bindable is TextInput) return TextInput.TextColorProperty;
                //if (bindable is TextAreaInput) return TextAreaInput.TextColorProperty;
                //if (bindable is DatePickerInput) return DatePickerInput.TextColorProperty;
                //if (bindable is PickerInput) return PickerInput.TextColorProperty;
                return null;
            }
        },
        { ManuelaProperty.TextDecoration, bindable =>
            {
                if (bindable is Label) return Label.TextDecorationsProperty;
                if (bindable is Span) return Span.TextDecorationsProperty;
                return null;
            }
        },
        { ManuelaProperty.VerticalOptions, bindable => View.VerticalOptionsProperty },
        { ManuelaProperty.HorizontalOptions, bindable => View.HorizontalOptionsProperty },
        { ManuelaProperty.Opacity, bindable => VisualElement.OpacityProperty },
        { ManuelaProperty.Width, bindable => VisualElement.WidthRequestProperty },
        { ManuelaProperty.Height, bindable => VisualElement.HeightRequestProperty },
        { ManuelaProperty.XAnchor, bindable => VisualElement.AnchorXProperty },
        { ManuelaProperty.YAnchor, bindable => VisualElement.AnchorYProperty },
        { ManuelaProperty.TranslateX, bindable => VisualElement.TranslationXProperty },
        { ManuelaProperty.TranslateY, bindable => VisualElement.TranslationYProperty },
        { ManuelaProperty.Rotation, bindable => VisualElement.RotationProperty },
        { ManuelaProperty.RotationX, bindable => VisualElement.RotationXProperty },
        { ManuelaProperty.RotationY, bindable => VisualElement.RotationYProperty },
        { ManuelaProperty.Scale, bindable => VisualElement.ScaleProperty },
        { ManuelaProperty.ScaleX, bindable => VisualElement.ScaleXProperty },
        { ManuelaProperty.ScaleY, bindable => VisualElement.ScaleYProperty }
    };
    private static readonly Dictionary<ManuelaProperty, Func<BindableObject, object?, object?>> s_converters = new()
    {
        { ManuelaProperty.Background, BrushConverter },
        { ManuelaProperty.BorderColor, BorderColorConverter },
        { ManuelaProperty.Shadow, ShadowConverter },
        { ManuelaProperty.TextColor, ColorConverter }
    };

    public static BindableProperty? GetBindableProperty(BindableObject? bindable, ManuelaProperty property)
    {
        if (bindable is null) return null;

        if (s_propertiesMap.TryGetValue(property, out var getter))
        {
            return getter(bindable);
        }

        return null;
    }

    public static object? TryConvert(BindableObject bindable, ManuelaProperty property, object? value)
    {
        if (s_converters.TryGetValue(property, out var converter))
        {
            return converter(bindable, value);
        }

        return value;
    }

    private static object? BrushConverter(BindableObject bindable, object? source)
    {
        if (source is null) return null;

        // if the source is already a brush, return it
        if (source is Brush brush) return brush;

        // otherwise, convert the source to a brush
        var uiBrush = (UIBrush)source;
        var intBrush = (int)uiBrush;

        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var colors = theme == AppTheme.Light
            ? Theme.Current.LightColors
            : Theme.Current.DarkColors;

        if ((intBrush & UICC.Gradient) > 0)
        {
            int sw1 = UICC.Sw400, sw2 = UICC.Sw600;

            if ((intBrush & UICC.GradientSmall) > 0)
            {
                sw1 = UICC.Sw500;
                sw2 = UICC.Sw600;
            }

            if ((intBrush & UICC.GradientLarge) > 0)
            {
                sw1 = UICC.Sw300;
                sw2 = UICC.Sw700;
            }

            Point start = new(0.5, 0);
            Point end = new(0.5, 1);

            if ((intBrush & UICC.GradientX) > 0)
            {
                start = new(0, 0.5);
                end = new(1, 0.5);
            }

            if ((intBrush & UICC.GradientY) > 0)
            {
                start = new(0.5, 0);
                end = new(0.5, 1);
            }

            var baseColor = (int)UIBrush.Primary;
            if ((uiBrush & UIBrush.Secondary) > 0) baseColor = (int)UIBrush.Secondary;
            if ((uiBrush & UIBrush.Tertiary) > 0) baseColor = (int)UIBrush.Tertiary;
            if ((uiBrush & UIBrush.Gray) > 0) baseColor = (int)UIBrush.Gray;

            var c1 = colors.Colors[(UIBrush)(baseColor | sw1)];
            var c2 = colors.Colors[(UIBrush)(baseColor | sw2)];

            return new LinearGradientBrush([new(c1, 0), new(c2, 1)], start, end);
        }

        return new SolidColorBrush(colors.Colors[uiBrush]);
    }

    private static object? ColorConverter(BindableObject bindable, object? source)
    {
        if (source is null) return null;

        // if the source is already a color, return it
        if (source is Color color) return color;

        // otherwise, convert the source to a brush
        var uiColor = (UIColor)source;

        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var colors = theme == AppTheme.Light
            ? Theme.Current.LightColors
            : Theme.Current.DarkColors;

        return colors.Colors[(UIBrush)uiColor];
    }

    private static object? BorderColorConverter(BindableObject bindable, object? source)
    {
        if (source is null || bindable is null) return null;

        if (bindable is Border) return BrushConverter(bindable, source);

        return ColorConverter(bindable, source);
    }

    private static object? ShadowConverter(BindableObject bindable, object? source)
    {
        if (source is null) return null;

        // if the source is already a shadow, return it
        if (source is Shadow shadow) return shadow;

        var uiSize = (UISize)source;

        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var shadows = theme == AppTheme.Light
            ? Theme.Current.ShadowLight
            : Theme.Current.ShadowDark;

        return shadows[uiSize];
    }
}

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
        "AllScreens", typeof(Set), typeof(On), null, propertyChanged: GetBreakpointStyleChangedDelegate(BreakPoint.all));
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

    public static BindableProperty ConditionalStatesProperty = BindableProperty.CreateAttached(
        "ConditionalStates", typeof(StatesCollection), typeof(On), null, propertyChanged: OnStateChanged);
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static Set GetAllScreens(BindableObject view) => (Set)view.GetValue(AllScreensProperty);
    public static void SetAllScreens(BindableObject view, Set value) => view.SetValue(AllScreensProperty, value);

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
