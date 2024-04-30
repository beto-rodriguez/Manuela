using Manuela;
using MauiIcons.SegoeFluent;

namespace Gallery.Layout;

public partial class AppMenuItem : StackLayout
{
    public AppMenuItem()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty RouteProperty = BindableProperty.Create(
        nameof(Route), typeof(string), typeof(AppMenuItem), null,
        propertyChanged: OnRoutePropertyChanged);

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(SegoeFluentIcons), typeof(AppMenuItem), null,
        propertyChanged: OnIconPropertyChanged);

    public static readonly BindableProperty DisplayProperty = BindableProperty.Create(
        nameof(Display), typeof(string), typeof(AppMenuItem), null,
        propertyChanged: OnDisplayPropertyChanged);

    public string Route
    {
        get => (string)GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    public SegoeFluentIcons? Icon
    {
        get => (SegoeFluentIcons?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Display
    {
        get => (string)GetValue(DisplayProperty);
        set => SetValue(DisplayProperty, value);
    }

    public event Action<StackLayout>? Tapped;

    private void OnTapped(object sender, TappedEventArgs e)
    {
        Tapped?.Invoke(this);
    }

    private static void OnRoutePropertyChanged(
        BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not AppMenuItem menuItem) return;
        Router.SetLink(menuItem, (string)newValue);
    }

    private static void OnIconPropertyChanged(
        BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not AppMenuItem menuItem) return;
        menuItem.icon.Icon = (SegoeFluentIcons?)newValue;
    }

    private static void OnDisplayPropertyChanged(
        BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not AppMenuItem menuItem) return;
        menuItem.label.Text = (string)newValue;
    }
}
