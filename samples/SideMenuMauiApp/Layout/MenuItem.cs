using MauiIcons.FontAwesome;

namespace SideMenuMauiApp.Layout;

public class MenuItemModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public FontAwesomeIcons Icon { get; set; }
}
