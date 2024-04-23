using Manuela.Styling;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Manuela.Forms;

public class CheckBoxInput : HorizontalStackLayout
{
    private bool _isDown;

    private readonly AbsoluteLayout _checkLayout;
    private readonly Border _checkBackground;
    private readonly Border _checkActiveBackground;
    private readonly Border _checkmark;
    private readonly Label _label;

    public CheckBoxInput()
    {
        _checkLayout = new() { StyleClass = new[] { "checkbox-layout" } };
        _checkBackground = new() { StyleClass = new[] { "checkbox-inactive-background" } };
        _checkActiveBackground = new() { StyleClass = new[] { "checkbox-active-background" }, Scale = 0 };

        var p = (Shape)new StrokeShapeTypeConverter().ConvertFrom("Path M4 12.6111L8.92308 17.5L20 6.5")!;

        _checkmark = new Border
        {
            StyleClass = new[] { "checkbox-checkmark" },
            StrokeShape = p,
            AnchorX = 0,
            AnchorY = 0,
            Scale = _checkLayout.HeightRequest / 28d, // the path is 28x28
            Opacity = 0,
            ZIndex = 99
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

        _checkLayout.Children.Add(_checkBackground);
        _checkLayout.Children.Add(_checkActiveBackground);
        _checkLayout.Children.Add(_checkmark);

        Children.Add(_checkLayout);
        Children.Add(_label = new Label { VerticalOptions = LayoutOptions.Center });

        var b = new Behaviors.Behavior(this);

        b.Down += OnDown;
        b.Up += OnUp;
    }

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(
            nameof(IsChecked), typeof(bool), typeof(CheckBoxInput), false, propertyChanged: OnIsCheckedChanged);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CheckBoxInput), string.Empty,
            propertyChanged: (BindableObject o, object old, object newVal) =>
                ((CheckBoxInput)o)._label.SetValue(Label.TextProperty, newVal));

    public event Action<CheckBoxInput>? CheckedChanged;

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
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
