using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views.CustromTriggers;

public partial class IsAnyChildEmpty : XamlState
{
    public VerticalStackLayout Layout { get; set; }

    public override bool IsActive(VisualElement visualElement)
    {
        return Layout?.Children.OfType<Entry>().Any(x => x.Text?.Length == 0) ?? false;
    }
}
