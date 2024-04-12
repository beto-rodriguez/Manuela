using Manuela.Styling;
using Manuela.Theming;

namespace Manuela.Things;

public static class ManuelaThings
{
    private static readonly Dictionary<ManuelaProperty, List<IBindablePropertySource>> s_propertiesMap = new()
    {
        [ManuelaProperty.Background] =
            [
                new PropertySource<CheckBox>(CheckBox.ColorProperty),
                new PropertySource<VisualElement>(VisualElement.BackgroundProperty)
            ],
        [ManuelaProperty.Margin] =
            [
                new PropertySource<View>(View.MarginProperty)
            ],
        [ManuelaProperty.Padding] =
            [
                new PropertySource<Border>(Border.PaddingProperty),
                new PropertySource<Button>(Button.PaddingProperty),
                new PropertySource<Microsoft.Maui.Controls.Compatibility.Layout>(Microsoft.Maui.Controls.Compatibility.Layout.PaddingProperty),
                new PropertySource<ImageButton>(ImageButton.PaddingProperty),
                new PropertySource<Label>(Label.PaddingProperty),
                new PropertySource<Layout>(Layout.PaddingProperty),
                new PropertySource<Page>(Page.PaddingProperty)
            ],
        [ManuelaProperty.BorderColor] =
            [
                new PropertySource<Border>(Border.StrokeProperty),
                new PropertySource<Button>(Button.BorderColorProperty),
                new PropertySource<Frame>(Frame.BorderColorProperty)
            ],
        [ManuelaProperty.BorderThickness] =
            [
                new PropertySource<Border>(Border.StrokeThicknessProperty),
                new PropertySource<Button>(Button.BorderWidthProperty)
            ],
        [ManuelaProperty.BorderRadius] =
            [
                new PropertySource<Border>(Border.StrokeShapeProperty),
                new PropertySource<Button>(Button.CornerRadiusProperty),
                new PropertySource<Frame>(Frame.CornerRadiusProperty),
                new PropertySource<BoxView>(BoxView.CornerRadiusProperty)
            ],
        [ManuelaProperty.Shadow] =
            [
                new PropertySource<VisualElement>(VisualElement.ShadowProperty)
            ],
        [ManuelaProperty.TextSize] =
            [
                new PropertySource<Button>(Button.FontSizeProperty),
                new PropertySource<DatePicker>(DatePicker.FontSizeProperty),
                new PropertySource<Editor>(Editor.FontSizeProperty),
                new PropertySource<Entry>(Entry.FontSizeProperty),
                new PropertySource<SearchBar>(SearchBar.FontSizeProperty),
                new PropertySource<Label>(Label.FontSizeProperty),
                new PropertySource<Picker>(Picker.FontSizeProperty),
                new PropertySource<RadioButton>(RadioButton.FontSizeProperty),
                new PropertySource<SearchHandler>(SearchHandler.FontSizeProperty),
                new PropertySource<Span>(Span.FontSizeProperty),
                new PropertySource<TimePicker>(TimePicker.FontSizeProperty)
            ],
        [ManuelaProperty.LineHeight] =
            [
                new PropertySource<Label>(Label.LineHeightProperty),
                new PropertySource<Span>(Span.LineHeightProperty)
            ],
        [ManuelaProperty.FontAttributes] =
            [
                new PropertySource<Button>(Button.FontAttributesProperty),
                new PropertySource<DatePicker>(DatePicker.FontAttributesProperty),
                new PropertySource<Editor>(Editor.FontAttributesProperty),
                new PropertySource<Entry>(Entry.FontAttributesProperty),
                new PropertySource<SearchBar>(SearchBar.FontAttributesProperty),
                new PropertySource<Label>(Label.FontAttributesProperty),
                new PropertySource<Picker>(Picker.FontAttributesProperty),
                new PropertySource<RadioButton>(RadioButton.FontAttributesProperty),
                new PropertySource<SearchHandler>(SearchHandler.FontAttributesProperty),
                new PropertySource<Span>(Span.FontAttributesProperty),
                new PropertySource<TimePicker>(TimePicker.FontAttributesProperty)
            ],
        [ManuelaProperty.VerticalTextAlign] =
            [
                new PropertySource<Label>(Label.VerticalTextAlignmentProperty)
            ],
        [ManuelaProperty.HorizontalTextAlign] =
            [
                new PropertySource<Label>(Label.HorizontalTextAlignmentProperty)
            ],
        [ManuelaProperty.TextColor] =
            [
                new PropertySource<Button>(Button.TextColorProperty),
                new PropertySource<DatePicker>(DatePicker.TextColorProperty),
                new PropertySource<Editor>(Editor.TextColorProperty),
                new PropertySource<Entry>(Entry.TextColorProperty),
                new PropertySource<SearchBar>(SearchBar.TextColorProperty),
                new PropertySource<Label>(Label.TextColorProperty),
                new PropertySource<Picker>(Picker.TextColorProperty),
                new PropertySource<SearchHandler>(SearchHandler.TextColorProperty),
                new PropertySource<Span>(Span.TextColorProperty),
                new PropertySource<RadioButton>(RadioButton.TextColorProperty),
                new PropertySource<TimePicker>(TimePicker.TextColorProperty)
            ],
        [ManuelaProperty.TextDecoration] =
            [
                new PropertySource<Label>(Label.TextDecorationsProperty),
                new PropertySource<Span>(Span.TextDecorationsProperty)
            ],
        [ManuelaProperty.VerticalOptions] =
            [
                new PropertySource<View>(View.VerticalOptionsProperty)
            ],
        [ManuelaProperty.HorizontalOptions] =
            [
                new PropertySource<View>(View.HorizontalOptionsProperty)
            ],
        [ManuelaProperty.Opacity] =
            [
                new PropertySource<VisualElement>(VisualElement.OpacityProperty)
            ],
        [ManuelaProperty.Width] =
            [
                new PropertySource<VisualElement>(VisualElement.WidthRequestProperty)
            ],
        [ManuelaProperty.Height] =
            [
                new PropertySource<VisualElement>(VisualElement.HeightRequestProperty)
            ],
        [ManuelaProperty.XAnchor] =
            [
                new PropertySource<VisualElement>(VisualElement.AnchorXProperty)
            ],
        [ManuelaProperty.YAnchor] =
            [
                new PropertySource<VisualElement>(VisualElement.AnchorYProperty)
            ],
        [ManuelaProperty.TranslateX] =
            [
                new PropertySource<VisualElement>(VisualElement.TranslationXProperty)
            ],
        [ManuelaProperty.TranslateY] =
            [
                new PropertySource<VisualElement>(VisualElement.TranslationYProperty)
            ],
        [ManuelaProperty.Rotation] =
            [
                new PropertySource<VisualElement>(VisualElement.RotationProperty)
            ],
        [ManuelaProperty.RotationX] =
            [
                new PropertySource<VisualElement>(VisualElement.RotationXProperty)
            ],
        [ManuelaProperty.RotationY] =
            [
                new PropertySource<VisualElement>(VisualElement.RotationYProperty)
            ],
        [ManuelaProperty.Scale] =
            [
                new PropertySource<VisualElement>(VisualElement.ScaleProperty)
            ],
        [ManuelaProperty.ScaleX] =
            [
                new PropertySource<VisualElement>(VisualElement.ScaleXProperty)
            ],
        [ManuelaProperty.ScaleY] =
            [
                new PropertySource<VisualElement>(VisualElement.ScaleYProperty)
            ],
        [ManuelaProperty.MaxWidth] =
            [
                new PropertySource<VisualElement>(VisualElement.MaximumWidthRequestProperty)
            ],
        [ManuelaProperty.MaxHeight] =
            [
                new PropertySource<VisualElement>(VisualElement.MaximumHeightRequestProperty)
            ],
        [ManuelaProperty.MinWidth] =
            [
                new PropertySource<VisualElement>(VisualElement.MinimumWidthRequestProperty)
            ],
        [ManuelaProperty.MinHeight] =
            [
                new PropertySource<VisualElement>(VisualElement.MinimumHeightRequestProperty)
            ],
        [ManuelaProperty.Visible] =
            [
                new PropertySource<VisualElement>(VisualElement.IsVisibleProperty)
            ],
        [ManuelaProperty.Style] =
            [
                new PropertySource<VisualElement>(VisualElement.StyleProperty)
            ],
        [ManuelaProperty.AbsoluteLayoutBounds] =
            [
                new PropertySource<VisualElement>(AbsoluteLayout.LayoutBoundsProperty)
            ],
        [ManuelaProperty.AbsoluteLayoutFlags] =
            [
                new PropertySource<VisualElement>(AbsoluteLayout.LayoutFlagsProperty)
            ],
        [ManuelaProperty.ImageSource] =
            [
                new PropertySource<ImageButton>(ImageButton.SourceProperty),
                new PropertySource<Image>(Image.SourceProperty)
            ],
        [ManuelaProperty.StackOrientation] =
            [
                new PropertySource<StackLayout>(StackLayout.OrientationProperty)
            ],
        [ManuelaProperty.StackSpacing] =
            [
                new PropertySource<StackBase>(StackBase.SpacingProperty)
            ],
        [ManuelaProperty.IsEnabled] =
            [
                new PropertySource<VisualElement>(VisualElement.IsEnabledProperty)
            ]
    };
    private static readonly Dictionary<ManuelaProperty, Func<BindableObject, object?, object?>> s_converters = new()
    {
        { ManuelaProperty.Background, BrushConverter },
        { ManuelaProperty.BorderColor, BorderColorConverter },
        { ManuelaProperty.Shadow, ShadowConverter },
        { ManuelaProperty.TextColor, ColorConverter },
        { ManuelaProperty.ImageSource, ImageSourceConverter }
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

        if (s_propertiesMap.TryGetValue(property, out var propertySources))
        {
            foreach (var propertySource in propertySources)
            {
                var bindableProperty = propertySource.Get(bindable);
                if (bindableProperty == null) continue;

                return bindableProperty;
            }
        }

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

    internal static Brush ConvertToBrush(int intFlags, ColorSet colors)
    {
        var baseColor = GetBaseColor(intFlags);

        if ((intFlags & UICC.Gradient) > 0)
        {
            int sw1 = UICC.Sw300, sw2 = UICC.Sw700;

            if ((intFlags & UICC.GradientSmall) > 0)
            {
                sw1 = UICC.Sw400;
                sw2 = UICC.Sw600;
            }

            if ((intFlags & UICC.GradientLarge) > 0)
            {
                sw1 = UICC.Sw100;
                sw2 = UICC.Sw800;
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

            if ((intFlags & UICC.GradientInvert) > 0) (c2, c1) = (c1, c2);

            c1 = SetColorOpacity(c1, intFlags);
            c2 = SetColorOpacity(c2, intFlags);

            return new LinearGradientBrush([new(c1, 0), new(c2, 1)], start, end);
        }

        var swatch = GetSwatch(intFlags);
        var c = colors.Colors[(UIBrush)(baseColor | swatch)];
        c = SetColorOpacity(c, intFlags);

        return new SolidColorBrush(c);
    }

    internal static Color ConvertToColor(int intFlags, ColorSet colors)
    {
        var baseColor = GetBaseColor(intFlags);
        var swatch = GetSwatch(intFlags);

        return colors.Colors[(UIBrush)(baseColor | swatch)];
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

        return ConvertToBrush(intFlags, colors);
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

        return SetColorOpacity(ConvertToColor(intFlags, colors), intFlags);
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

        var s = Theme.Current.Shadows[uiSize];

        return theme == AppTheme.Light ? s.Light : s.Dark;
    }

    private static object? ImageSourceConverter(BindableObject bindable, object? source)
    {
        if (source is null) return null;

        var strValue = source?.ToString();

        if (strValue is null)
        {
            throw new InvalidOperationException(
                string.Format("Cannot convert \"{0}\" into {1}", strValue, typeof(ImageSource)));
        }

        return Uri.TryCreate(strValue, UriKind.Absolute, out var uri) && uri.Scheme != "file"
            ? ImageSource.FromUri(uri)
            : ImageSource.FromFile(strValue);
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

        if ((flags & UICC.Opacity05) > 0) opacity = 0.05f;
        if ((flags & UICC.Opacity10) > 0) opacity = 0.1f;
        if ((flags & UICC.Opacity20) > 0) opacity = 0.2f;
        if ((flags & UICC.Opacity35) > 0) opacity = 0.35f;
        if ((flags & UICC.Opacity50) > 0) opacity = 0.5f;
        if ((flags & UICC.Opacity65) > 0) opacity = 0.65f;
        if ((flags & UICC.Opacity80) > 0) opacity = 0.8f;
        if ((flags & UICC.Opacity90) > 0) opacity = 0.9f;
        if ((flags & UICC.Opacity95) > 0) opacity = 0.95f;

        return color.MultiplyAlpha(opacity);
    }

    public static void RegisterType<T>(ManuelaProperty property, BindableProperty bindableProperty)
    {
        s_propertiesMap[property].Add(new PropertySource<T>(bindableProperty));
    }
}
