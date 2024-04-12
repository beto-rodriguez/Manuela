namespace Manuela.Expressions;

public class XamlCondition(Func<VisualElement, bool> predicate)
{
#pragma warning disable IDE1006 // Naming Styles
    internal static XamlCondition Empty = new(v => false);
#pragma warning restore IDE1006 // Naming Styles

    public Func<VisualElement, bool> Predicate { get; } = predicate;
    public Trigger[] Triggers { get; set; } = [];
}
