using Manuela.States;

namespace Gallery.Views.CustomStates;

// custom states are experimental.
// objects that inherit from XamlState will use Manuela's source generator.
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
