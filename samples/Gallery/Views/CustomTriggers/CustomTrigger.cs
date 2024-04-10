using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views.CustromTriggers;

public partial class IsEmpty : XamlState
{
    public Entry? Entry { get; set; }

    public override bool IsActive(VisualElement visualElement)
    {
        return Entry?.Text?.Length == 0;
    }

    protected override void OnInitialized(VisualElement visual)
    {
        Condition = new Manuela.Expressions.XamlCondition(v => true);
    }
}
