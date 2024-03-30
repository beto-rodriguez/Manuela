using Manuela.Styling;
using Manuela.Theming;

namespace Manuela.Things;

public static class ManuelaThings
{
    private static readonly Dictionary<ManuelaProperty, Func<BindableObject, BindableProperty?>> s_propertiesMap = new()
    {
        { ManuelaProperty.Background, bindable =>
            {
                if (bindable is CheckBox) return CheckBox.ColorProperty;
                return VisualElement.BackgroundProperty;
            }
        },
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
        { ManuelaProperty.ScaleY, bindable => VisualElement.ScaleYProperty },
        { ManuelaProperty.MaxWidth, bindable => VisualElement.MaximumWidthRequestProperty },
        { ManuelaProperty.MaxHeight, bindable => VisualElement.MaximumHeightRequestProperty },
        { ManuelaProperty.MinWidth, bindable => VisualElement.MinimumWidthRequestProperty },
        { ManuelaProperty.MinHeight, bindable => VisualElement.MinimumHeightRequestProperty },
        { ManuelaProperty.Visible, bindable => VisualElement.IsVisibleProperty },
        { ManuelaProperty.Style, bindable => VisualElement.StyleProperty },
        { ManuelaProperty.AbsoluteLayoutBounds, bindable => AbsoluteLayout.LayoutBoundsProperty }
    };
    private static readonly Dictionary<ManuelaProperty, Func<BindableObject, object?, object?>> s_converters = new()
    {
        { ManuelaProperty.Background, BrushConverter },
        { ManuelaProperty.BorderColor, BorderColorConverter },
        { ManuelaProperty.Shadow, ShadowConverter },
        { ManuelaProperty.TextColor, ColorConverter }
    };
    private static readonly Dictionary<Type, Func<BindableObject, BindableProperty, object, Animation>> s_transitions = new()
    {
        { typeof(double), (bindable, property, targeValue) =>
            {
                var start = (double)bindable.GetValue(property);
                var end = (double)targeValue;
                return new(t =>
                    bindable.SetValue(
                        property,
                        start + t * (end - start)),
                        0,
                        1);
            }
        },
        { typeof(Brush), (bindable, property, targeValue) =>
            {
                var start = bindable.GetValue(property);
                var end = targeValue;

                if (start is SolidColorBrush startBrush && end is SolidColorBrush endBrush)
                {
                    var s = startBrush.Color;
                    var e = endBrush.Color;

                    s ??= e;

                    return new(t =>
                        startBrush.Color = Color.FromRgba(
                            s.Red + t * (e.Red - s.Red),
                            s.Green + t * (e.Green - s.Green),
                            s.Blue + t * (e.Blue - s.Blue),
                            s.Alpha + t * (e.Alpha - s.Alpha)));
                    // NOTE #1
                    // maybe better performance? set a new instance of?
                    //bindable.SetValue(
                    //    property,
                    //    new SolidColorBrush(Color.FromRgba(
                    //        s.Red + t * (e.Red - s.Red),
                    //        s.Green + t * (e.Green - s.Green),
                    //        s.Blue + t * (e.Blue - s.Blue),
                    //        s.Alpha + t * (e.Alpha - s.Alpha)))),
                    //    0,
                    //    1);
                }

                throw new NotImplementedException("Only Solid color brush is supported for now...");
            }
        },
        { typeof(Color), (bindable, property, targeValue) =>
            {
                var start = (Color)bindable.GetValue(property);
                var end = (Color)targeValue;

                return new(t =>
                    bindable.SetValue(
                        property,
                        Color.FromRgba(
                            start.Red + t * (end.Red - start.Red),
                            start.Green + t * (end.Green - start.Green),
                            start.Blue + t * (end.Blue - start.Blue),
                            start.Alpha + t * (end.Alpha - start.Alpha))),
                        0,
                        1);
            }
        },
        { typeof(Thickness), (bindable, property, targeValue) =>
            {
                var start = (Thickness)bindable.GetValue(property);
                var end = (Thickness)targeValue;

                return new(t =>
                    bindable.SetValue(
                        property,
                        new Thickness(
                            start.Left + t * (end.Left - start.Left),
                            start.Top + t * (end.Top - start.Top),
                            start.Right + t * (end.Right - start.Right),
                            start.Bottom + t * (end.Bottom - start.Bottom))),
                        0,
                        1);
            }
        },
        { typeof(Rect), (bindable, property, targetValue) =>
            {
                var start = (Rect)bindable.GetValue(property);
                var end = (Rect)targetValue;

                return new(t =>
                    bindable.SetValue(
                        property,
                        new Rect(
                            start.X + t * (end.X - start.X),
                            start.Y + t * (end.Y - start.Y),
                            start.Width + t * (end.Width - start.Width),
                            start.Height + t * (end.Height - start.Height))),
                        0,
                        1);
            }
        },
        { typeof(Shadow), (bindable, property, targeValue) =>
            {
                var start = (Shadow)bindable.GetValue(property);
                var end = (Shadow)targeValue;

                // see NOTE #1... same question
                // we are also just animating the offset, blur and opacity... not the color.
                return new(t =>
                    {
                        start.Radius = (float)(start.Radius + t * (end.Radius - start.Radius));
                        start.Offset = new(
                            start.Offset.X + t * (end.Offset.X - start.Offset.X),
                            start.Offset.Y + t * (end.Offset.Y - start.Offset.Y));
                        start.Opacity = (float)(start.Opacity + t * (end.Opacity - start.Opacity));
                    },
                    0,
                    1);
            }
        }
    };

    public static BindableProperty? GetBindableProperty(BindableObject? bindable, ManuelaProperty property)
    {
        if (bindable is null) return null;

        if (s_propertiesMap.TryGetValue(property, out var getter))
            return getter(bindable);

        return null;
    }

    public static object? TryConvert(BindableObject bindable, ManuelaProperty property, object? value)
    {
        if (s_converters.TryGetValue(property, out var converter))
            return converter(bindable, value);

        return value;
    }

    public static Animation GetAnimation(BindableObject bindable, BindableProperty property, object targetValue)
    {
        if (s_transitions.TryGetValue(property.ReturnType, out var animationBuilder))
            return animationBuilder(bindable, property, targetValue);

        throw new NotImplementedException($"Transition for {property.ReturnType} is not supported.");
    }

    private static object? BrushConverter(BindableObject bindable, object? source)
    {
        if (source is null) return null;

        // if the source is already a brush, return it
        if (source is Brush brush) return brush;

        // exception for CheckBox....
        if (bindable is CheckBox) return ColorConverter(bindable, source);

        // otherwise, convert the source to a brush
        var uiBrush = (UIBrush)source;
        var intFlags = (int)uiBrush;

        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var colors = theme == AppTheme.Light
            ? Theme.Current.LightColors
            : Theme.Current.DarkColors;

        var baseColor = GetBaseColor(intFlags);

        if ((intFlags & UICC.Gradient) > 0)
        {
            int sw1 = UICC.Sw400, sw2 = UICC.Sw600;

            if ((intFlags & UICC.GradientSmall) > 0)
            {
                sw1 = UICC.Sw500;
                sw2 = UICC.Sw600;
            }

            if ((intFlags & UICC.GradientLarge) > 0)
            {
                sw1 = UICC.Sw300;
                sw2 = UICC.Sw700;
            }

            Point start = new(0.5, 0);
            Point end = new(0.5, 1);

            if ((intFlags & UICC.GradientX) > 0)
            {
                start = new(0, 0.5);
                end = new(1, 0.5);
            }

            if ((intFlags & UICC.GradientY) > 0)
            {
                start = new(0.5, 0);
                end = new(0.5, 1);
            }

            var c1 = colors.Colors[(UIBrush)(baseColor | sw1)];
            var c2 = colors.Colors[(UIBrush)(baseColor | sw2)];

            c1 = SetColorOpacity(c1, intFlags);
            c2 = SetColorOpacity(c2, intFlags);

            return new LinearGradientBrush([new(c1, 0), new(c2, 1)], start, end);
        }

        var swatch = GetSwatch(intFlags);

        return new SolidColorBrush(SetColorOpacity(colors.Colors[(UIBrush)(baseColor | swatch)], intFlags));
    }

    private static object? ColorConverter(BindableObject bindable, object? source)
    {
        if (source is null) return null;

        // if the source is already a color, return it
        if (source is Color color) return color;

        // otherwise, convert the source to a brush
        var uiColor = (UIColor)source;
        var intFlags = (int)uiColor;

        var theme = Application.Current?.RequestedTheme;
        if (theme is null or AppTheme.Unspecified) theme = AppTheme.Light;

        var colors = theme == AppTheme.Light
            ? Theme.Current.LightColors
            : Theme.Current.DarkColors;

        var baseColor = GetBaseColor(intFlags);
        var sw = GetSwatch(intFlags);

        var c = colors.Colors[(UIBrush)(baseColor | sw)];
        c = SetColorOpacity(c, intFlags);

        return c;
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

    private static int GetBaseColor(int intFlags)
    {
        var baseColor = UICC.Primary;

        if ((intFlags & UICC.Secondary) > 0) baseColor = UICC.Secondary;
        if ((intFlags & UICC.Tertiary) > 0) baseColor = UICC.Tertiary;
        if ((intFlags & UICC.Gray) > 0) baseColor = UICC.Gray;

        return baseColor;
    }

    private static int GetSwatch(int intFlags)
    {
        var swatch = 0;

        if ((intFlags & UICC.Sw50) > 0) swatch = UICC.Sw50;
        if ((intFlags & UICC.Sw100) > 0) swatch = UICC.Sw100;
        if ((intFlags & UICC.Sw200) > 0) swatch = UICC.Sw200;
        if ((intFlags & UICC.Sw300) > 0) swatch = UICC.Sw300;
        if ((intFlags & UICC.Sw400) > 0) swatch = UICC.Sw400;
        if ((intFlags & UICC.Sw500) > 0) swatch = UICC.Sw500;
        if ((intFlags & UICC.Sw600) > 0) swatch = UICC.Sw600;
        if ((intFlags & UICC.Sw700) > 0) swatch = UICC.Sw700;
        if ((intFlags & UICC.Sw800) > 0) swatch = UICC.Sw800;
        if ((intFlags & UICC.Sw900) > 0) swatch = UICC.Sw900;
        if ((intFlags & UICC.Sw950) > 0) swatch = UICC.Sw950;

        return swatch;
    }

    private static Color SetColorOpacity(Color color, int flags)
    {
        var opacity = 1f;

        if ((flags & UICC.Opacity0) > 0) opacity = 0f;
        if ((flags & UICC.Opacity10) > 0) opacity = 0.1f;
        if ((flags & UICC.Opacity20) > 0) opacity = 0.2f;
        if ((flags & UICC.Opacity30) > 0) opacity = 0.3f;
        if ((flags & UICC.Opacity40) > 0) opacity = 0.4f;
        if ((flags & UICC.Opacity50) > 0) opacity = 0.5f;
        if ((flags & UICC.Opacity60) > 0) opacity = 0.6f;
        if ((flags & UICC.Opacity70) > 0) opacity = 0.7f;
        if ((flags & UICC.Opacity80) > 0) opacity = 0.8f;
        if ((flags & UICC.Opacity90) > 0) opacity = 0.9f;
        if ((flags & UICC.Opacity100) > 0) opacity = 1f;

        return color.MultiplyAlpha(opacity);
    }

    public static void SetPointerPassthroughRegion(Region[] regions, Window[]? windows = null)
    {
#if WINDOWS
        windows ??= Application.Current?.Windows.ToArray() ?? [];

        foreach (var window in windows)
        {
            if (window.Handler.PlatformView is not Microsoft.UI.Xaml.Window w) continue;

            var nonClientInputSrc = Microsoft.UI.Input.InputNonClientPointerSource.GetForWindowId(w.AppWindow.Id);

            nonClientInputSrc.SetRegionRects(
                Microsoft.UI.Input.NonClientRegionKind.Passthrough,
                regions.Select(x => x.ToRectInt32()).ToArray());
        }
#endif
    }
}
