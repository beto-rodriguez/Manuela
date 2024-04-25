using System.Collections;
using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class PickerInput : BaseInput<Picker, IPickerHandler>
{
    public PickerInput()
    {
        BaseControl.BackgroundColor = Colors.Transparent;
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(PickerInput), typeof(IList), typeof(Picker), default(IList),
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((PickerInput)o).BaseControl.SetValue(Picker.ItemsSourceProperty, newVal));

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(PickerInput), Colors.Black,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((PickerInput)o).BaseControl.SetValue(Picker.TextColorProperty, newVal));

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(PickerInput), 14d,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((PickerInput)o).BaseControl.SetValue(Picker.FontSizeProperty, newVal));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(PickerInput), FontAttributes.None,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((PickerInput)o).BaseControl.SetValue(Picker.FontAttributesProperty, newVal));

    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

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

    protected override bool CanRestoreLabelOnUnFocus => BaseControl.SelectedItem is null;

    protected override void OnInputHandlerChanged(IPickerHandler handler)
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
    }
}
