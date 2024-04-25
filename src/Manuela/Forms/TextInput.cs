using System.Windows.Input;
using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class TextInput : BaseInput<Entry, IEntryHandler>, IInputControl
{
    public TextInput()
    {
        BaseControl.BackgroundColor = Colors.Transparent;

        ValueChanged += (_, _) =>
        {
            SetValue(ValueProperty, BaseControl.Text);
            ValueChangedCommand?.Execute(Value);
        };
    }

    public static readonly BindableProperty InputProperty =
        BindableProperty.Create(nameof(BaseControl), typeof(PropertyInput), typeof(TextInput), null,
            propertyChanged: OnInputChanged);

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(string), typeof(TextInput), string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty ValueChangedCommandProperty =
        BindableProperty.Create(nameof(ValueChangedCommand), typeof(ICommand), typeof(TextInput), null);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TextInput), Colors.Black,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextInput)o).BaseControl.SetValue(Entry.TextColorProperty, newVal));

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TextInput), 14d,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextInput)o).BaseControl.SetValue(Entry.FontSizeProperty, newVal));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(TextInput), FontAttributes.None,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextInput)o).BaseControl.SetValue(Entry.FontAttributesProperty, newVal));

    public PropertyInput Input
    {
        get => (PropertyInput)GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public ICommand ValueChangedCommand
    {
        get => (ICommand)GetValue(ValueChangedCommandProperty);
        set => SetValue(ValueChangedCommandProperty, value);
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

    public event EventHandler<TextChangedEventArgs> ValueChanged
    {
        add => BaseControl.TextChanged += value;
        remove => BaseControl.TextChanged -= value;
    }

    void IInputControl.SetValue(object? value)
    {
        BaseControl.Text = (string?)value ?? string.Empty;
    }

    void IInputControl.SetPlaceholder(string placeholder)
    {
        if (string.IsNullOrWhiteSpace(placeholder)) return;

        Placeholder = placeholder;
        SetInputFocus(speed: 1);
    }

    protected override bool CanRestoreLabelOnUnFocus => string.IsNullOrWhiteSpace(BaseControl.Text);

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

    private static void OnInputChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (newvalue is PropertyInput input)
        {
            input.Initialize((TextInput)bindable);
        }
    }
}
