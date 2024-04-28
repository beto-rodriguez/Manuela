using System.Windows.Input;
using Manuela.Styling;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Manuela.Forms;

public class CheckBoxInput : VerticalStackLayout, IInputControl
{
    private bool _isInitialized;
    private bool _isDown;

    private readonly AbsoluteLayout _checkLayout;
    private readonly Border _checkBackground;
    private readonly Border _checkActiveBackground;
    private readonly Border _checkmark;
    private readonly Label _label;
    private readonly Label _validationLabel;

    public CheckBoxInput()
    {
        _isInitialized = true;

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

        var horizontalLayout = new HorizontalStackLayout { Spacing = 12 };

        horizontalLayout.Children.Add(_checkLayout);
        horizontalLayout.Children.Add(_label = new Label { VerticalOptions = LayoutOptions.Center });

        _validationLabel = new Label
        {
            StyleClass = new[] { "validation-message" },
            IsVisible = ValidationMessage.Length > 0
        };

        Children.Add(horizontalLayout);
        Children.Add(_validationLabel);

        var b = new Behaviors.Behavior(this);

        b.Down += OnDown;
        b.Up += OnUp;
    }

    #region Bindable Properties

    public static readonly BindableProperty ForProperty =
       BindableProperty.Create(nameof(For), typeof(PropertyInput), typeof(CheckBoxInput), null,
           propertyChanged: OnInputChanged);

    public static readonly BindableProperty ValueChangedCommandProperty =
        BindableProperty.Create(nameof(IInputControl.ValueChangedCommand), typeof(ICommand), typeof(CheckBoxInput), null);

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(
            nameof(Value), typeof(bool), typeof(CheckBoxInput), false, propertyChanged: OnIsCheckedChanged);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CheckBoxInput), string.Empty,
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var input = (CheckBoxInput)bindable;
                if (!input._isInitialized) return;
                input._label.SetValue(Label.TextProperty, newValue);
            });

    public static readonly BindableProperty ValidationMessageProperty =
        BindableProperty.Create(
            nameof(ValidationMessage), typeof(string), typeof(CheckBoxInput), string.Empty,
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var input = (CheckBoxInput)bindable;
                if (!input._isInitialized) return;
                var newStr = (string?)newValue;
                var isValid = string.IsNullOrWhiteSpace(newStr);
                input._validationLabel.Text = newStr;
                input._validationLabel.IsVisible = !isValid;
                input.StyleClass = isValid ? ["input-valid"] : ["input-invalid"];
            });

    #endregion

    public event Action<CheckBoxInput>? CheckedChanged;

    #region properties

    /// <summary>
    /// Binds the validation and the input to the control.
    /// </summary>
    public PropertyInput For
    {
        get => (PropertyInput)GetValue(ForProperty);
        set => SetValue(ForProperty, value);
    }

    ICommand IInputControl.ValueChangedCommand
    {
        get => (ICommand)GetValue(ValueChangedCommandProperty);
        set => SetValue(ValueChangedCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the checked state of the checkbox.
    /// </summary>
    public bool Value
    {
        get => (bool)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the placeholder text of the input.
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// Gets or sets the validation message of the input.
    /// </summary>
    public string ValidationMessage
    {
        get => (string)GetValue(ValidationMessageProperty);
        set => SetValue(ValidationMessageProperty, value);
    }

    #endregion

    void IInputControl.SetValue(object? value) => Value = (bool?)value ?? false;
    void IInputControl.SetPlaceholder(string placeholder) { }
    void IInputControl.Dispatch(Action action) => Dispatcher.Dispatch(action);

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

        Value = !Value;
        _isDown = false;

        _checkActiveBackground.SetManuelaProperty(ManuelaProperty.Scale, Value ? 1d : 0d);
        _checkmark.Opacity = Value ? 1 : 0;
    }

    private static void OnInputChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (newvalue is not PropertyInput input) return;

        input.Initialize((CheckBoxInput)bindable);
    }
}
