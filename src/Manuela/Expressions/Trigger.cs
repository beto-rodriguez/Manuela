﻿using System.ComponentModel;

namespace Manuela.Expressions;

public class Trigger(INotifyPropertyChanged notifier, HashSet<string> properties)
{
    public INotifyPropertyChanged Notifier { get; } = notifier;
    public HashSet<string> Properties { get; } = properties;
    internal PropertyChangedEventHandler? NotifierHandler { get; set; }
}
