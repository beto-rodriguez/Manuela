namespace Manuela.Forms;

public class InputGroup : Border
{
    private readonly ContentView _left;
    private readonly ContentView _right;
    private readonly ContentView _middle;

    public InputGroup()
    {
        _middle = new() { VerticalOptions = LayoutOptions.Center };
        _left = new() { VerticalOptions = LayoutOptions.Center };
        _right = new() { VerticalOptions = LayoutOptions.Center };

        Grid.SetColumn(_left, 0);
        Grid.SetColumn(_middle, 1);
        Grid.SetColumn(_right, 2);

        Content = new Grid
        {
            ColumnDefinitions =
            [
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
            ],
            Children =
            {
                _left,
                _middle,
                _right
            }
        };

        SizeChanged += (_, _) => Content.WidthRequest = Width;
    }

    public static readonly BindableProperty MiddleContentProperty =
       BindableProperty.Create(
           nameof(MiddleContent), typeof(View), typeof(InputGroup), null,
           propertyChanged: OnMiddleChanged);

    public static readonly BindableProperty LeftContentProperty =
        BindableProperty.Create(
            nameof(LeftContent), typeof(View), typeof(InputGroup), null,
            propertyChanged: GetItemsChangedHandler(Side.Left));

    public static readonly BindableProperty RightContentProperty =
       BindableProperty.Create(
           nameof(RightContent), typeof(View), typeof(InputGroup), null,
           propertyChanged: GetItemsChangedHandler(Side.Right));

    public View MiddleContent
    {
        get => (View)GetValue(MiddleContentProperty);
        set => SetValue(MiddleContentProperty, value);
    }

    public View LeftContent
    {
        get => (View)GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public View RightContent
    {
        get => (View)GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    private static BindableProperty.BindingPropertyChangedDelegate GetItemsChangedHandler(Side side)
    {
        return (bindable, oldVal, newVal) =>
        {
            if (newVal is not View view) return;

            var inputGroup = (InputGroup)bindable;
            var contentView = side == Side.Left ? inputGroup._left : inputGroup._right;
            contentView.Content = view;
        };
    }

    private static void OnMiddleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is not View view) return;

        var inputGroup = (InputGroup)bindable;

        view.Margin = new Thickness(0);
        inputGroup._middle.Content = view;
    }

    private enum Side
    {
        Left,
        Right
    }
}
