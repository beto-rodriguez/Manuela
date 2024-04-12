using System.ComponentModel;

namespace Manuela.States;

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

    public static IEnumerable<T> Listen<T, U>(this IEnumerable<T> source, Func<T, U> predicate)
    {
        // a dummy iteration to trigger the predicate.
        // this is only necesary once, when the state is initialized.
        // we could improve this method by removing the foreach in the source generator.

        foreach (var item in source)
        {
            _ = predicate(item);
        }

        return source;
    }
}
