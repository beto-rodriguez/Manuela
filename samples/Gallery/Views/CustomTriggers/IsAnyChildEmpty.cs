using System.ComponentModel;
using Manuela.Styling.ConditionalStyles;

namespace Gallery.Views.CustromTriggers;

public partial class IsAnyChildEmpty : XamlState
{
    public VerticalStackLayout Layout { get; set; }

    public override bool IsActive(VisualElement visualElement)
    {
        return Layout.Children.OfType<Entry>().Any(x => x.Text?.Length == 0);
    }

    protected override void OnInitialized(VisualElement visual)
    {
        Condition = new Manuela.Expressions.XamlCondition(IsActive);
        var triggers = new Dictionary<INotifyPropertyChanged, Manuela.Expressions.Trigger>();
        var evaluation = GetNotifiers(visual, triggers);
        Condition.Triggers = [.. triggers.Values];
    }

    protected void OnInitialized2()
    {
        Condition = new(IsActive);
    }

    private bool GetNotifiers(
        VisualElement visualElement,
        Dictionary<INotifyPropertyChanged, Manuela.Expressions.Trigger> triggers)
    {
        return Layout.Children.OfType<Entry>().Any(x => Notify(x, "Text", triggers).Text?.Length == 0);
    }

    private static T Notify<T>(
        T notifier,
        string propertyName,
        Dictionary<INotifyPropertyChanged, Manuela.Expressions.Trigger> triggers)
            where T : INotifyPropertyChanged
    {
        if (!triggers.TryGetValue(notifier, out var trigger))
            triggers.Add(notifier, trigger = new(notifier, [propertyName]));

        _ = trigger.Properties.Add(propertyName);

        return notifier;
    }
}
