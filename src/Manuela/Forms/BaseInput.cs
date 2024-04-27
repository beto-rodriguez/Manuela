using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Manuela.Forms;

public abstract class BaseInput<TInput, TValue, THandler> : Border, IInputControl
    where TInput : View, new()
    where THandler : IViewHandler
{
    private bool _isInitialized;

    protected Label _label;
    protected AbsoluteLayout _inputLayout;
    protected BoxView _activeBoxView;
    protected Label _validationLabel;

    public BaseInput()
    {
        _isInitialized = true;

        _inputLayout = new AbsoluteLayout { MinimumHeightRequest = InputMinimumHeightRequest };

        var cornerRadius = StrokeShape is RoundRectangle roundRectangle ? roundRectangle.CornerRadius.TopLeft : 0;

        _label = new Label
        {
            AnchorX = 0,
            Padding = new(14, 0),
            Opacity = LabelOpacity,
            TextColor = PlaceholderColor
        };

        AbsoluteLayout.SetLayoutFlags(_label, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(_label, new(0, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        var input = new TInput { Margin = new(4, 0) };

        AbsoluteLayout.SetLayoutFlags(input, AbsoluteLayoutFlags.SizeProportional | AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(input, new(0, 0, 1, 1));

        _activeBoxView = new BoxView { BackgroundColor = HighlightColor, Margin = new(cornerRadius / 2f, 0) };
        AbsoluteLayout.SetLayoutFlags(_activeBoxView, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(_activeBoxView, new(0.5f, 1, 0, HighlightBorderHeight));

        input.HandlerChanged += Input_HandlerChanged;

        _inputLayout.Children.Add(_label);
        _inputLayout.Children.Add(input);
        _inputLayout.Children.Add(_activeBoxView);

        _validationLabel = new Label
        {
            StyleClass = new[] { "validation-message" },
            IsVisible = ValidationMessage.Length > 0
        };

        var content = new Grid
        {
            RowDefinitions =
            [
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            ]
        };

        content.Children.Add(_inputLayout);
        content.Children.Add(_validationLabel);

        Grid.SetRow(_inputLayout, 0);
        Grid.SetRow(_validationLabel, 1);

        Content = content;

        BaseControl = input;

        BaseControl.Focused += (_, _) => SetInputFocus();
        BaseControl.Unfocused += (_, _) => RemoveInputFocus();
    }

    #region bindable properties

    public static readonly BindableProperty ForProperty =
        BindableProperty.Create(nameof(For), typeof(PropertyInput), typeof(BaseInput<TInput, TValue, THandler>), null,
            propertyChanged: OnInputChanged);

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(TValue), typeof(BaseInput<TInput, TValue, THandler>), default(TValue),
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty ValueChangedCommandProperty =
        BindableProperty.Create(nameof(IInputControl.ValueChangedCommand), typeof(ICommand), typeof(BaseInput<TInput, TValue, THandler>), null);

    public static readonly BindableProperty InputMinimumHeightRequestProperty =
        BindableProperty.Create(
         nameof(InputMinimumHeightRequest), typeof(double), typeof(BaseInput<TInput, TValue, THandler>), 46d,
         propertyChanged: GetOnChanged((i, v) => i._inputLayout.MinimumHeightRequest = (double)v));

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder), typeof(string), typeof(BaseInput<TInput, TValue, THandler>), null,
            propertyChanged: GetOnChanged((i, v) =>
            {
                var newLabel = (string?)v;
                i._label.Text = newLabel;
                i.BaseControl.TranslationY = newLabel is not null && newLabel.Length > 0 ? 6 : 0;
            }));

    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(
            nameof(PlaceholderColor), typeof(Color), typeof(BaseInput<TInput, TValue, THandler>), Color.FromRgba(0, 0, 0, 255),
            propertyChanged: GetOnChanged((i, v) => i._label.TextColor = (Color)v));

    public static readonly BindableProperty PlaceholderOpacityProperty =
        BindableProperty.Create(
            nameof(LabelOpacity), typeof(double), typeof(BaseInput<TInput, TValue, THandler>), 0.8d,
            propertyChanged: GetOnChanged((i, v) => i._label.Opacity = (double)v));

    public static readonly BindableProperty HighlightColorProperty =
        BindableProperty.Create(
            nameof(HighlightColor), typeof(Color), typeof(BaseInput<TInput, TValue, THandler>), Color.FromRgba(59, 130, 246, 0),
            propertyChanged: GetOnChanged((i, v) => i._activeBoxView.BackgroundColor = (Color)v));

    public static readonly BindableProperty HighlightBorderHeightProperty =
        BindableProperty.Create(
            propertyName: nameof(HighlightBorderHeight), returnType: typeof(double),
            declaringType: typeof(BaseInput<TInput, TValue, THandler>), defaultValue: 3d);

    public static readonly BindableProperty ValidationMessageProperty =
        BindableProperty.Create(
            nameof(ValidationMessage), typeof(string), typeof(BaseInput<TInput, TValue, THandler>), string.Empty,
            propertyChanged: GetOnChanged((i, v) =>
            {
                var newStr = (string?)v;
                i._validationLabel.Text = newStr;
                i._validationLabel.IsVisible = newStr?.Length > 0;
            }));

    public static readonly BindableProperty TextColorProperty =
       BindableProperty.Create(
           nameof(TextColor), typeof(Color), typeof(BaseInput<TInput, TValue, THandler>), Colors.Black,
           propertyChanged: GetOnChanged((i, v) => i.BaseControl.SetValue(i.GetTextColorProperty(), v)));

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize), typeof(double), typeof(BaseInput<TInput, TValue, THandler>), 14d,
            propertyChanged: GetOnChanged((i, v) => i.BaseControl.SetValue(i.GetFontSizeProperty(), v)));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(BaseInput<TInput, TValue, THandler>), FontAttributes.None,
        propertyChanged: GetOnChanged((i, v) => i.BaseControl.SetValue(i.GetFontAttributesProperty(), v)));

    #endregion

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
    /// Gets or sets the value of the input.
    /// </summary>
    public TValue Value
    {
        get => (TValue)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets the base control.
    /// </summary>
    public TInput BaseControl { get; }

    public double InputMinimumHeightRequest
    {
        get { return (double)GetValue(InputMinimumHeightRequestProperty); }
        set { SetValue(InputMinimumHeightRequestProperty, value); }
    }

    /// <summary>
    /// Gets or sets the text of the label.
    /// </summary>
    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    /// <summary>
    /// Gets or sets the text color of the label.
    /// </summary>
    public Color PlaceholderColor
    {
        get { return (Color)GetValue(PlaceholderColorProperty); }
        set { SetValue(PlaceholderColorProperty, value); }
    }

    /// <summary>
    /// Gets or sets the opacity of the label.
    /// </summary>
    public double LabelOpacity
    {
        get { return (double)GetValue(PlaceholderOpacityProperty); }
        set { SetValue(PlaceholderOpacityProperty, value); }
    }

    /// <summary>
    /// Gets or sets the color of to highlight the input.
    /// </summary>
    public Color HighlightColor
    {
        get { return (Color)GetValue(HighlightColorProperty); }
        set { SetValue(HighlightColorProperty, value); }
    }

    /// <summary>
    /// Gets or sets the height of the highlight.
    /// </summary>
    public double HighlightBorderHeight
    {
        get { return (double)GetValue(HighlightBorderHeightProperty); }
        set { SetValue(HighlightBorderHeightProperty, value); }
    }

    /// <summary>
    /// Gets or sets the validation message.
    /// </summary>
    public string ValidationMessage
    {
        get { return (string)GetValue(ValidationMessageProperty); }
        set { SetValue(ValidationMessageProperty, value); }
    }

    /// <summary>
    /// Gets or sets the text color of the input.
    /// </summary>
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the input.
    /// </summary>
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font attributes of the input.
    /// </summary>
    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    #endregion

    protected abstract bool CanRestoreLabelOnUnFocus { get; }

    protected struct LabelTransform
    {
        public Rect Bounds { get; set; }
        public double Scale { get; set; }
        public Thickness Margin { get; set; }
    }

    protected void Transform(
        bool canTransformLabel,
        LabelTransform labelTransform,
        bool canTransformBoxView,
        Rect boxViewBounds,
        uint length = 150)
    {
        if (_label is null) return;

        var startLabelScale = _label.Scale;
        var startLabelBounds = AbsoluteLayout.GetLayoutBounds(_label);
        var startLabelMargin = _label.Margin;
        var startBoxViewBounds = AbsoluteLayout.GetLayoutBounds(_activeBoxView);

        var labelEndScale = labelTransform.Scale;
        var labelEndBounds = labelTransform.Bounds;
        var labelEndMargin = labelTransform.Margin;

        new Animation(
            t =>
            {
                if (canTransformLabel)
                {
                    _label.Scale = startLabelScale + t * (labelEndScale - startLabelScale);
                    _label.Margin = new(
                        startLabelMargin.Left + t * (labelEndMargin.Left - startLabelMargin.Left),
                        startLabelMargin.Top + t * (labelEndMargin.Top - startLabelMargin.Top),
                        startLabelMargin.Right + t * (labelEndMargin.Right - startLabelMargin.Right),
                        startLabelMargin.Bottom + t * (labelEndMargin.Bottom - startLabelMargin.Bottom));
                    AbsoluteLayout.SetLayoutBounds(
                        _label,
                        new Rect(
                            startLabelBounds.X + t * (labelEndBounds.X - startLabelBounds.X),
                            startLabelBounds.Y + t * (labelEndBounds.Y - startLabelBounds.Y),
                            startLabelBounds.Width + t * (labelEndBounds.Width - startLabelBounds.Width),
                            startLabelBounds.Height + t * (labelEndBounds.Height - startLabelBounds.Height)));
                }

                if (canTransformBoxView)
                {
                    AbsoluteLayout.SetLayoutBounds(
                        _activeBoxView,
                        new Rect(
                            startBoxViewBounds.X + t * (boxViewBounds.X - startBoxViewBounds.X),
                            startBoxViewBounds.Y + t * (boxViewBounds.Y - startBoxViewBounds.Y),
                            startBoxViewBounds.Width + t * (boxViewBounds.Width - startBoxViewBounds.Width),
                            startBoxViewBounds.Height + t * (boxViewBounds.Height - startBoxViewBounds.Height)));
                }
            }).Commit(this, "Transform", 16, length, Easing.CubicOut);
    }

    protected abstract void OnInputHandlerChanged(THandler handler);
    protected abstract void SetInputValue(object? value);
    protected abstract BindableProperty GetTextColorProperty();
    protected abstract BindableProperty GetFontSizeProperty();
    protected abstract BindableProperty GetFontAttributesProperty();

    public virtual void SetInputFocus(uint speed = 150, bool? transformLabel = null, bool? transformViewBox = null)
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

    public virtual void RemoveInputFocus(uint speed = 150, bool? transformLabel = null, bool? transformViewBox = null)
    {
        Transform(
            transformLabel ?? CanRestoreLabelOnUnFocus,
            new LabelTransform
            {
                Bounds = new(0, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
                Scale = 1,
                Margin = new(0),
            },
            transformViewBox ?? true,
            new(0.5f, 1, 0, HighlightBorderHeight),
            speed);
    }

    void IInputControl.SetValue(object? value)
    {
        SetInputValue(value);
    }

    void IInputControl.SetPlaceholder(string placeholder)
    {
        if (string.IsNullOrWhiteSpace(placeholder)) return;
        Placeholder = placeholder;
        if (!CanRestoreLabelOnUnFocus) SetInputFocus(speed: 1);
    }

    private void Input_HandlerChanged(object? sender, EventArgs e)
    {
        if (sender is not TInput input || input.Handler is null || input.Handler.PlatformView is null)
            return;

        OnInputHandlerChanged((THandler)input.Handler);
    }

    private static void OnInputChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (newvalue is not PropertyInput input) return;

        input.Initialize((BaseInput<TInput, TValue, THandler>)bindable);
    }

    private static BindableProperty.BindingPropertyChangedDelegate GetOnChanged(
        Action<BaseInput<TInput, TValue, THandler>, object> predicate)
    {
        return (bindable, oldValue, newValue) =>
        {
            if (bindable is not BaseInput<TInput, TValue, THandler> input || !input._isInitialized) return;
            predicate(input, newValue);
        };
    }
}
