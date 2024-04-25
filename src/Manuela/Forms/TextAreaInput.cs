using Microsoft.Maui.Handlers;

namespace Manuela.Forms;

public class TextAreaInput : BaseInput<Editor, IEditorHandler>
{
    public TextAreaInput()
    {
        _label.Margin = new(0, 14, 0, 0);
        AbsoluteLayout.SetLayoutBounds(_label, new(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        BaseControl.BackgroundColor = Colors.Transparent;
        BaseControl.Margin = new(7, 14, 7, 0);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TextAreaInput), Colors.Black,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextAreaInput)o).BaseControl.SetValue(Editor.TextColorProperty, newVal));

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TextAreaInput), 14d,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextAreaInput)o).BaseControl.SetValue(Editor.FontSizeProperty, newVal));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(TextAreaInput), FontAttributes.None,
        propertyChanged: (BindableObject o, object old, object newVal) =>
            ((TextAreaInput)o).BaseControl.SetValue(Editor.FontAttributesProperty, newVal));

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

    protected override bool CanRestoreLabelOnUnFocus => string.IsNullOrWhiteSpace(BaseControl.Text);

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

