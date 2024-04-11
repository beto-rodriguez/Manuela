using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views.CustomStates;

// ccustom states are experimental.
// Objects that inherit from XamlState will use Manuela's source generator.
// it generates the necessary code to make the trigger work.
// source generatos could fail if the "IsActive" method is too complex.

public partial class IsEmpty : XamlState
{
    public Entry? Entry { get; set; }

    public override bool IsActive(VisualElement visualElement)
    {
        return string.IsNullOrEmpty(Entry?.Text);
    }
}
