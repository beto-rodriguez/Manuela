﻿using System.Collections;
using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class PickerInput : BaseInput<Picker, object, IPickerHandler>
{
    public PickerInput()
    {
        BaseControl.BackgroundColor = Colors.Transparent;
#if MACCATALYST || IOS
        BaseControl.Margin = new(15, 0);
#endif
        ValueChanged += (_, _) =>
        {
            var newValue = BaseControl.SelectedItem;
            SetValue(ValueProperty, newValue);
            ((IInputControl)this).InputValueChangedCommand?.Execute(newValue);

            if (ValueChangedCommand is not null && ValueChangedCommand.CanExecute(newValue))
                ValueChangedCommand?.Execute(newValue);

            SetInputFocus(transformViewBox: false); // move placeholder on selection change
        };
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(PickerInput), typeof(IList), typeof(Picker), default(IList),
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((PickerInput)o).BaseControl.SetValue(Picker.ItemsSourceProperty, newVal));

    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public event EventHandler ValueChanged
    {
        add => BaseControl.SelectedIndexChanged += value;
        remove => BaseControl.SelectedIndexChanged -= value;
    }

    protected override bool CanRestoreLabelOnUnFocus => BaseControl.SelectedItem is null;
    protected override void SetInputValue(object? value)
    {
        BaseControl.SelectedItem = value;
    }

    protected override BindableProperty GetTextColorProperty()
    {
        return Picker.TextColorProperty;
    }

    protected override BindableProperty GetFontSizeProperty()
    {
        return Picker.FontSizeProperty;
    }

    protected override BindableProperty GetFontAttributesProperty()
    {
        return Picker.FontAttributesProperty;
    }

    protected override void OnInputHandlerChanged(IPickerHandler handler)
    {
#if ANDROID
        handler.PlatformView.BackgroundTintList =
            Android.Content.Res.ColorStateList.ValueOf(
                Microsoft.Maui.Controls.Compatibility.Platform.Android.ColorExtensions.ToAndroid(Colors.Transparent));
#elif IOS && !MACCATALYST
        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif MACCATALYST
        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        handler.PlatformView.Style = null;
#endif
    }
}
