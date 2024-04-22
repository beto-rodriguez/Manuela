using Manuela.Styling;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Manuela.Forms;

public class CheckBoxInput : AbsoluteLayout
{
    private bool _isDown;

    private readonly Border _checkBackground;
    private readonly Border _checkActiveBackground;
    private readonly Border _checkmark;

    public CheckBoxInput()
    {
        _checkBackground = new() { StyleClass = new[] { "CheckBoxInputBackground" } };
        _checkActiveBackground = new() { StyleClass = new[] { "CheckBoxInputActiveBackground" }, Scale = 0 };

        var p = (Shape)new StrokeShapeTypeConverter().ConvertFrom("Path M4 12.6111L8.92308 17.5L20 6.5")!;

        _checkmark = new Border
        {
            StyleClass = new[] { "CheckBoxInputCheck" },

            StrokeShape = p,
            AnchorX = 0,
            AnchorY = 0,
            Scale = HeightRequest / 28d,
            Opacity = 0
        };

        Has.SetTransitions(_checkActiveBackground, [
            new Transition { Property = ManuelaProperty.Scale, Duration = 150, Easing = Easing.CubicOut }
        ]);

        _checkActiveBackground.SetManuelaProperty(ManuelaProperty.Scale, 0d);

        AbsoluteLayout.SetLayoutFlags(_checkBackground, AbsoluteLayoutFlags.SizeProportional);
        AbsoluteLayout.SetLayoutFlags(_checkActiveBackground, AbsoluteLayoutFlags.SizeProportional);
        AbsoluteLayout.SetLayoutFlags(_checkmark, AbsoluteLayoutFlags.SizeProportional);

        AbsoluteLayout.SetLayoutBounds(_checkBackground, new(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutBounds(_checkActiveBackground, new(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutBounds(_checkmark, new(0, 0, 1, 1));

        Children.Add(_checkBackground);
        Children.Add(_checkActiveBackground);
        Children.Add(_checkmark);

        var b = new Behaviors.Behavior(this);

        b.Down += OnDown;
        b.Up += OnUp;
    }

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(
            nameof(IsChecked), typeof(bool), typeof(CheckBoxInput), false, propertyChanged: OnIsCheckedChanged);

    public event Action<CheckBoxInput>? CheckedChanged;

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var checkBox = (CheckBoxInput)bindable;
        checkBox.CheckedChanged?.Invoke(checkBox);
    }

    private void OnDown()
    {
        _isDown = true;
    }

    private void OnUp()
    {
        // if the pointer was not down in this control, ignore the up event
        if (!_isDown) return;

        IsChecked = !IsChecked;
        _isDown = false;

        _checkActiveBackground.SetManuelaProperty(ManuelaProperty.Scale, IsChecked ? 1d : 0d);
        _checkmark.Opacity = IsChecked ? 1 : 0;
    }
}
