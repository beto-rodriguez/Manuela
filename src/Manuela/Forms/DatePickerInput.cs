using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class DatePickerInput : BaseInput<DatePicker, DateTime, IDatePickerHandler>
{
    public DatePickerInput()
    {
        BaseControl.BackgroundColor = Colors.Transparent;
        
        #if MACCATALYST && !IOS
        BaseControl.Margin = new(0, 15, 0, 0);
        #endif

        ValueChanged += (_, _) =>
        {
            var newValue = BaseControl.Date;
            SetValue(ValueProperty, newValue);

            // at loast on windows, the date picker DateSelected event is called on initialization.
            // lets prevent this ValueChanged call when the control is not loaded yet.

            if (BaseControl.IsLoaded) ((IInputControl)this).ValueChangedCommand?.Execute(newValue);
        };
    }

    public event EventHandler<DateChangedEventArgs> ValueChanged
    {
        add => BaseControl.DateSelected += value;
        remove => BaseControl.DateSelected -= value;
    }

    protected override bool CanRestoreLabelOnUnFocus => false;
    protected override void SetInputValue(object? value) => BaseControl.Date = (DateTime?)value ?? DateTime.MinValue;
    protected override BindableProperty GetTextColorProperty() => DatePicker.TextColorProperty;
    protected override BindableProperty GetFontSizeProperty() => DatePicker.FontSizeProperty;
    protected override BindableProperty GetFontAttributesProperty() => DatePicker.FontAttributesProperty;

    protected override void OnInputHandlerChanged(IDatePickerHandler handler)
    {
#if ANDROID
        handler.PlatformView.BackgroundTintList =
            Android.Content.Res.ColorStateList.ValueOf(
                Microsoft.Maui.Controls.Compatibility.Platform.Android.ColorExtensions.ToAndroid(Colors.Transparent));
#elif IOS && !MACCATALYST
        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif MACCATALYST
        // already borderless? at least on macos 14.4
#elif WINDOWS
        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        handler.PlatformView.Style = null;
#endif
        SetInputFocus(transformViewBox: false);
    }
}
