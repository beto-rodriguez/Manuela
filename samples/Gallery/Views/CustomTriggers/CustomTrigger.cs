using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views.CustromTriggers;

public partial class IsEmpty : XamlState
{
    public Entry? Entry { get; set; }

    public override bool IsActive(VisualElement visualElement)
    {
        return string.IsNullOrEmpty(Entry?.Text);
    }
}
