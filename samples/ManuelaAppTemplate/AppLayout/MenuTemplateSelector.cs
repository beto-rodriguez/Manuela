namespace ManuelaAppTemplate.AppLayout;

public class MenuTemplateSelector : DataTemplateSelector
{
    public DataTemplate Item { get; set; } = null!;
    public DataTemplate Collapsed{ get; set; } = null!;

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return item is IMenuItem i && i.IsCollapseButton
            ? Collapsed
            : Item;
    }
}
