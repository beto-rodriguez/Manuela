using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class TextAreaInput : BaseInput<Editor, string, IEditorHandler>
{
    public TextAreaInput()
    {
        _label.Margin = new(0, 14, 0, 0);
        AbsoluteLayout.SetLayoutBounds(_label, new(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        BaseControl.BackgroundColor = Colors.Transparent;
        BaseControl.Margin = new(7, 14, 7, 0);

        ValueChanged += (_, _) =>
        {
            var newValue = BaseControl.Text;
            SetValue(ValueProperty, newValue);
            ((IInputControl)this).ValueChangedCommand?.Execute(newValue);
        };
    }

    public event EventHandler<TextChangedEventArgs> ValueChanged
    {
        add => BaseControl.TextChanged += value;
        remove => BaseControl.TextChanged -= value;
    }

    protected override bool CanRestoreLabelOnUnFocus => string.IsNullOrWhiteSpace(BaseControl.Text);
    protected override void SetInputValue(object? value) => BaseControl.Text = (string?)value ?? string.Empty;
    protected override BindableProperty GetTextColorProperty() => Editor.TextColorProperty;
    protected override BindableProperty GetFontSizeProperty() => Editor.FontSizeProperty;
    protected override BindableProperty GetFontAttributesProperty() => Editor.FontAttributesProperty;

    public override void SetInputFocus(uint speed = 150, bool? transformLabel = null, bool? transformViewBox = null)
    {
        Transform(
            transformLabel ?? true,
            new LabelTransform
            {
                Bounds = new(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
                Scale = 0.7,
                Margin = new(5, 0)
            },
            transformViewBox ?? true,
            new(0.5f, 1, 1, HighlightBorderHeight),
            speed);
    }

    public override void RemoveInputFocus(uint speed = 150, bool? transformLabel = null, bool? transformViewBox = null)
    {
        Transform(
            transformLabel ?? CanRestoreLabelOnUnFocus,
            new LabelTransform
            {
                Bounds = new(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
                Scale = 1,
                Margin = new(0, 14, 0, 0),
            },
            transformViewBox ?? true,
            new(0.5f, 1, 0, HighlightBorderHeight),
            speed);
    }

    protected override void OnInputHandlerChanged(IEditorHandler handler)
    {
#if IOS17_0_OR_GREATER || MACCATALYST17_0_OR_GREATER
        handler.PlatformView.BorderStyle = UIKit.UITextViewBorderStyle.None;
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

