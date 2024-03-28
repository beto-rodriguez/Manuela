namespace Manuela.Expressions;

public class XamlCondition(Func<VisualElement, bool> predicate)
{
    public Func<VisualElement, bool> Predicate { get; } = predicate;
    public Func<VisualElement, Trigger[]>? Triggers { get; set; }
}
