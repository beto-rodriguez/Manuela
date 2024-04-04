using Manuela.Styling;

namespace SideMenuMauiApp.Layout;

public partial class AppMenu : Border
{
    public AppMenu()
    {
        InitializeComponent();
    }

    public event Action? ItemTapped;

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        var clickedVisual = (HorizontalStackLayout)sender;
        var index = MenuItemsContainer.Children.IndexOf(clickedVisual);
        var itemsHeight = clickedVisual.Height;

        SelectedIndicator.SetManuelaProperty(
            ManuelaProperty.AbsoluteLayoutBounds,
            new Rect(0, itemsHeight * index, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

        ItemTapped?.Invoke();
    }
}
