using System.ComponentModel;

namespace Manuela.Expressions;

public class ConditionUpdateTrigger(INotifyPropertyChanged notifier, HashSet<string> properties)
{
    public INotifyPropertyChanged Notifier { get; } = notifier;
    public HashSet<string> Properties { get; } = properties;
}
