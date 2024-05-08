using MauiIcons.SegoeFluent;

namespace SideMenuMauiApp.LayoutComponents;

public interface IMenuItem
{
    public SegoeFluentIcons Icon { get; }
    public string RouteName { get; }
    public string Display { get; }
}
