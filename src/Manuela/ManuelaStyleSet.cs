using Manuela.Theming;

namespace Manuela;

public class ManuelaStyleSet
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
        }
    };
    private static readonly Dictionary<ManuelaProperty, Func<BindableObject, object?, object?>> s_converters = new()
    {
        { ManuelaProperty.Background, BrushConverter },
        { ManuelaProperty.BorderColor, BorderColorConverter },
        { ManuelaProperty.Shadow, ShadowConverter },
        { ManuelaProperty.TextColor, ColorConverter }
    };

    public ManuelaStyleSet()
    {
        ActiveBreakPoint = BreakPoint.unknown;
        IsInitialized = false;
    }

    public static object Unset { get; } = new();
    public BreakPoint ActiveBreakPoint { get; set; }
    public BindableObject? BindableObject { get; set; }
    public bool IsInitialized { get; set; }
    public ManuelaStyle? All { get; set; }
    public ManuelaStyle? Sm { get; set; }
    public ManuelaStyle? Md { get; set; }
    public ManuelaStyle? Lg { get; set; }
    public ManuelaStyle? Xl { get; set; }
    public ManuelaStyle? Xxl { get; set; }

    public void Apply(BreakPoint p, BindableObject bindable)
    {
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

            var bindableProperty = GetBindableProperty(property, bindable);
            var value = Get(property, p, bindable);

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
    }

    private object? Get(ManuelaProperty property, BreakPoint p, BindableObject bindable)
    {
        var value = Unset;

        if (All?.TryGetValue(property, out var all) ?? false) value = all;

        if (p >= BreakPoint.sm && (Sm?.TryGetValue(property, out var sm) ?? false)) value = sm;
        if (p >= BreakPoint.md && (Md?.TryGetValue(property, out var md) ?? false)) value = md;
        if (p >= BreakPoint.lg && (Lg?.TryGetValue(property, out var lg) ?? false)) value = lg;
        if (p >= BreakPoint.xl && (Xl?.TryGetValue(property, out var xl) ?? false)) value = xl;
        if (p >= BreakPoint.xxl && (Xxl?.TryGetValue(property, out var xxl) ?? false)) value = xxl;

        if (value == Unset) return value;

        if (s_converters.TryGetValue(property, out var converter))
        {
            return converter(bindable, value);
        }

        return value;
    }

    private static BindableProperty? GetBindableProperty(ManuelaProperty property, BindableObject bindable)
    {
        if (s_propertiesMap.TryGetValue(property, out var getter))
        {
            return getter(bindable);
        }

        return null;
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

            Point start = new(0, 0);
            Point end = new(1, 1);

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
        if (source is null) return null;

        if (bindable is Border) return BrushConverter(bindable, source);

        return ColorConverter(bindable, source);
    }

    private static object? ShadowConverter(BindableObject bindable, object? source)
    {
        if (source is null) return null;

        var uiSize = (UISize)source;

        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var shadows = theme == AppTheme.Light
            ? Theme.Current.ShadowLight
            : Theme.Current.ShadowDark;

        return shadows[uiSize];
    }
}
