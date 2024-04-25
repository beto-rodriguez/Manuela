using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Manuela.Forms;

public abstract class BaseInput<TInput, THandler> : Border
    where TInput : View, new()
    where THandler : IViewHandler
{
    private readonly bool _isInitialized;
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
            FontSize = 15,
            Text = ValidationMessage,
            Padding = new(14, 5),
            TextColor = Colors.Red,
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

    public static readonly BindableProperty InputMinimumHeightRequestProperty =
        BindableProperty.Create(
         propertyName: nameof(InputMinimumHeightRequest), returnType: typeof(double),
         declaringType: typeof(BaseInput<TInput, THandler>), defaultValue: 46d,
         propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
         {
             var input = (BaseInput<TInput, THandler>)bindable;
             if (!input._isInitialized) return;
             input._inputLayout.MinimumHeightRequest = (double)newValue;
         });

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            propertyName: nameof(Placeholder), returnType: typeof(string),
            declaringType: typeof(BaseInput<TInput, THandler>), defaultValue: null,
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var input = (BaseInput<TInput, THandler>)bindable;
                if (!input._isInitialized) return;
                var newLabel = (string?)newValue;
                input._label.Text = newLabel;
                input.BaseControl.TranslationY = newLabel is not null && newLabel.Length > 0 ? 6 : 0;
            });

    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(
            propertyName: nameof(PlaceholderColor), returnType: typeof(Color),
            declaringType: typeof(BaseInput<TInput, THandler>), defaultValue: Color.FromRgba(0, 0, 0, 255),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var input = (BaseInput<TInput, THandler>)bindable;
                if (!input._isInitialized) return;

                input._label.TextColor = (Color)newValue;
            });

    public static readonly BindableProperty PlaceholderOpacityProperty =
        BindableProperty.Create(
            propertyName: nameof(LabelOpacity), returnType: typeof(double),
            declaringType: typeof(BaseInput<TInput, THandler>), defaultValue: 0.8d,
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var input = (BaseInput<TInput, THandler>)bindable;
                if (!input._isInitialized) return;

                input._label.Opacity = (double)newValue;
            });

    public static readonly BindableProperty HighlightColorProperty =
        BindableProperty.Create(
            propertyName: nameof(HighlightColor), returnType: typeof(Color),
            declaringType: typeof(BaseInput<TInput, THandler>), defaultValue: Color.FromRgba(59, 130, 246, 0),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var input = (BaseInput<TInput, THandler>)bindable;
                if (!input._isInitialized) return;

                input._activeBoxView.BackgroundColor = (Color)newValue;
            });

    public static readonly BindableProperty HighlightBorderHeightProperty =
        BindableProperty.Create(
            propertyName: nameof(HighlightBorderHeight), returnType: typeof(double),
            declaringType: typeof(BaseInput<TInput, THandler>), defaultValue: 3d);

    public static readonly BindableProperty ValidationMessageProperty =
        BindableProperty.Create(
            propertyName: nameof(ValidationMessage), returnType: typeof(string),
            declaringType: typeof(BaseInput<TInput, THandler>), defaultValue: string.Empty,
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var input = (BaseInput<TInput, THandler>)bindable;
                if (!input._isInitialized) return;

                var newStr = (string?)newValue;
                input._validationLabel.Text = newStr;
                input._validationLabel.IsVisible = newStr?.Length > 0;
            });

    #endregion

    #region properties

    /// <summary>
    /// Gets the input control.
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

    private void Input_HandlerChanged(object? sender, EventArgs e)
    {
        if (sender is not TInput input || input.Handler is null || input.Handler.PlatformView is null)
            return;

        OnInputHandlerChanged((THandler)input.Handler);
    }
}
