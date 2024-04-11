using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views.CustomStates;

// custom states are experimental.
// Objects that inherit from XamlState will use Manuela's source generator.
// it generates the necessary code to make the trigger work.
// source generatos could fail if the "IsActive" method is too complex.

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
