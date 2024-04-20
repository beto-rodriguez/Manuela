using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class TextInput : BaseInput<Entry, IEntryHandler>
{
    public TextInput()
    {
        Input.BackgroundColor = Colors.Transparent;
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TextInput), Colors.Black,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextInput)o).Input.SetValue(Entry.TextColorProperty, newVal));

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TextInput), 14d,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextInput)o).Input.SetValue(Entry.FontSizeProperty, newVal));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(TextInput), FontAttributes.None,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextInput)o).Input.SetValue(Entry.FontAttributesProperty, newVal));

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

    protected override bool CanRestoreLabelOnUnFocus => string.IsNullOrWhiteSpace(Input.Text);

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
