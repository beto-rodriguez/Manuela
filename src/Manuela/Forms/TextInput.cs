using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class TextInput : BaseInput<Entry, string, IEntryHandler>
{
    public TextInput()
    {
        BaseControl.BackgroundColor = Colors.Transparent;

#if MACCATALYST || IOS
        BaseControl.Margin = new(15, 0);
#else
        BaseControl.Margin = new(4, 0);
#endif

        ValueChanged += (_, _) =>
        {
            var newValue = BaseControl.Text;
            SetValue(ValueProperty, newValue);
            ((IInputControl)this).InputValueChangedCommand?.Execute(newValue);

            if (ValueChangedCommand is not null && ValueChangedCommand.CanExecute(newValue))
                ValueChangedCommand?.Execute(newValue);
        };
    }

    public event EventHandler<TextChangedEventArgs> ValueChanged
    {
        add => BaseControl.TextChanged += value;
        remove => BaseControl.TextChanged -= value;
    }

    protected override bool CanRestoreLabelOnUnFocus => string.IsNullOrWhiteSpace(BaseControl.Text);
    protected override void SetInputValue(object? value)
    {
        BaseControl.Text = (string?)value ?? string.Empty;
    }

    protected override BindableProperty GetTextColorProperty()
    {
        return Entry.TextColorProperty;
    }

    protected override BindableProperty GetFontSizeProperty()
    {
        return Entry.FontSizeProperty;
    }

    protected override BindableProperty GetFontAttributesProperty()
    {
        return Entry.FontAttributesProperty;
    }

    protected override void OnInputHandlerChanged(IEntryHandler handler)
    {
#if IOS || MACCATALYST
        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif ANDROID
        handler.PlatformView.BackgroundTintList =
            Android.Content.Res.ColorStateList.ValueOf(
                Microsoft.Maui.Controls.Compatibility.Platform.Android.ColorExtensions.ToAndroid(Colors.Transparent));
#elif WINDOWS
        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        handler.PlatformView.Style = null;
#endif
    }
}
