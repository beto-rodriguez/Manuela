using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views;

public partial class MyCustomState : XamlState
{
    public Entry? TargetEntry { get; set; }

    public override bool IsActive(VisualElement visualElement)
    {
        return TargetEntry?.Text?.Length > 0;
    }
}
