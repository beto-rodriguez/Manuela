using Manuela.Styling;
using Microsoft.Maui.Controls.Shapes;

namespace Manuela.Forms;

public class RadioButtonInput : RadioButton
{
    private Border _background = null!;
    private Ellipse _check = null!;

    public RadioButtonInput()
    {
        CheckedChanged += OnCheckedChanged;
    }

    public static BindableProperty HighlightBrushProperty = BindableProperty.Create(
        nameof(HighlightBrush), typeof(Brush), typeof(RadioButtonInput), null);

    public static BindableProperty InactiveBrushProperty = BindableProperty.Create(
        nameof(InactiveBrush), typeof(Brush), typeof(RadioButtonInput), null);

    public Brush HighlightBrush
    {
        get => (Brush)GetValue(HighlightBrushProperty);
        set => SetValue(HighlightBrushProperty, value);
    }

    public Brush InactiveBrush
    {
        get => (Brush)GetValue(InactiveBrushProperty);
        set => SetValue(InactiveBrushProperty, value);
    }

    private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (_background is null || _check is null) return;
        UpdateVisualState();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _background = GetTemplateChild("background") as Border ??
            throw new Exception($"background not found in {nameof(RadioButtonInput)} control template.");
        _check = GetTemplateChild("check") as Ellipse ??
            throw new Exception($"check not found in {nameof(RadioButtonInput)} control template.");

        Has.SetTransitions(_check, [
            new Transition { Property = ManuelaProperty.Scale, Duration = 250, Easing = Easing.SinOut }
        ]);

        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (_background is null || _check is null) return;

        if (IsChecked)
        {
            _background.SetManuelaProperty(ManuelaProperty.Background, HighlightBrush);
            _check.SetManuelaProperty(ManuelaProperty.Scale, 1d);
        }
        else
        {
            _background.SetManuelaProperty(ManuelaProperty.Background, InactiveBrush);
            _check.SetManuelaProperty(ManuelaProperty.Scale, 0d);
        }
    }
}
