using System.ComponentModel;

namespace Manuela.Styling.ConditionalStyles;

public static class GeneratorExtensions
{
    public static T Notify<T>(
        this T notifier,
        string propertyName,
        Dictionary<INotifyPropertyChanged, Expressions.Trigger> triggers)
            where T : INotifyPropertyChanged
    {
        if (!triggers.TryGetValue(notifier, out var trigger))
            triggers.Add(notifier, trigger = new(notifier, [propertyName]));

        _ = trigger.Properties.Add(propertyName);

        return notifier;
    }
}
