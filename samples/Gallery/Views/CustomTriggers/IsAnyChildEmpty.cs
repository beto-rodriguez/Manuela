using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views.CustromTriggers;

public partial class IsAnyChildEmpty : XamlState
{
    public VerticalStackLayout Layout { get; set; }

    public override bool IsActive(VisualElement visualElement)
    {
        return Layout.Children
            .OfType<Entry>()
            .Listen(x => x.Text) // ensures that the states is re-evaluated when any child text changes.
            .Any(x => string.IsNullOrEmpty(x.Text));
    }
}
