using Manuela;
using MauiIcons.SegoeFluent;

namespace Gallery.Layout;

public partial class MoreMenuItem : HorizontalStackLayout
{
    public MoreMenuItem()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty RouteProperty = BindableProperty.Create(
        nameof(Route), typeof(string), typeof(MenuItem), null,
        propertyChanged: OnRoutePropertyChanged);

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(SegoeFluentIcons), typeof(MenuItem), null,
        propertyChanged: OnIconPropertyChanged);

    public static readonly BindableProperty DisplayProperty = BindableProperty.Create(
        nameof(Display), typeof(string), typeof(MenuItem), null,
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

    public event Action<HorizontalStackLayout>? Tapped;

    private void OnTapped(object sender, TappedEventArgs e)
    {
        Tapped?.Invoke(this);
    }

    private static void OnRoutePropertyChanged(
        BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not MoreMenuItem menuItem) return;
        Router.SetLink(menuItem, (string)newValue);
    }

    private static void OnIconPropertyChanged(
        BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not MoreMenuItem menuItem) return;
        menuItem.icon.Icon = (SegoeFluentIcons?)newValue;
    }

    private static void OnDisplayPropertyChanged(
        BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not MoreMenuItem menuItem) return;
        menuItem.label.Text = (string)newValue;
    }
}
