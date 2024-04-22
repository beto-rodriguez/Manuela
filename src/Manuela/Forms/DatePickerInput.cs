using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class DatePickerInput : BaseInput<DatePicker, IDatePickerHandler>
{
    public DatePickerInput()
    {
        Input.BackgroundColor = Colors.Transparent;
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(DatePickerInput), Colors.Black,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((DatePickerInput)o).Input.SetValue(DatePicker.TextColorProperty, newVal));

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(DatePickerInput), 14d,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((DatePickerInput)o).Input.SetValue(DatePicker.FontSizeProperty, newVal));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(DatePickerInput), FontAttributes.None,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((DatePickerInput)o).Input.SetValue(DatePicker.FontAttributesProperty, newVal));

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    protected override bool CanRestoreLabelOnUnFocus => false;

    protected override void OnInputHandlerChanged(IDatePickerHandler handler)
    {
#if ANDROID
        handler.PlatformView.BackgroundTintList =
            Android.Content.Res.ColorStateList.ValueOf(
                Microsoft.Maui.Controls.Compatibility.Platform.Android.ColorExtensions.ToAndroid(Colors.Transparent));
#elif IOS && !MACCATALYST
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif MACCATALYST
        // how?
#elif WINDOWS
            handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
            handler.PlatformView.Style = null;
#endif
        SetInputFocus(transformViewBox: false);
    }
}
