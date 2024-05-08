using MauiIcons.SegoeFluent;

namespace ManuelaAppTemplate.AppLayout;

public interface IMenuItem
{
    public SegoeFluentIcons Icon { get; }
    public string RouteName { get; }
    public string Display { get; }
    public IList<string>? StyleClass { get; }
    public bool IsCollapseButton { get; }
    public AppCollapsedMenu? CollapsedMenu { get; set; }
    List<IMenuItem> CollapsedOptions { get; set; }
}
