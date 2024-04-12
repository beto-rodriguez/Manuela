// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Manuela.Styling;
using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class StatesCollection : ObservableCollection<ConditionalStyle>
{
    private bool _initialized;
    private VisualElement? _visualElement;

    public StatesCollection()
    { }

    public StatesCollection(List<ConditionalStyle> list) : base(list) { }

    public bool ApplyPropertyIfMet(
        VisualElement visual,
        ManuelaProperty property,
        BindableProperty bindableProperty,
        TransitionsCollection? transitions)
    {
        // as soon as one condition is met, we can stop
        // we should be applying on the same property multiple times.

        foreach (var conditionalStyle in this)
        {
            var conditionMet = conditionalStyle.ApplyPropertyIfMet(visual, property, bindableProperty, transitions);
            if (conditionMet) return true;
        }

        return false;
    }

    public void Initialize(VisualElement visualElement)
    {
        if (_initialized) return;
        _visualElement = visualElement;
        _initialized = true;
    }

    public void Dispose()
    {
        _visualElement = null;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);

        if (_visualElement is null) return;

        var empty = Array.Empty<ConditionalStyle>();

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var newStyle in e.NewItems?.Cast<ConditionalStyle>() ?? empty)
                {
                    newStyle.Initialize(_visualElement);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var oldStyle in e.OldItems?.Cast<ConditionalStyle>() ?? empty)
                {
                    oldStyle.Dispose(_visualElement);
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                _visualElement.SetValue(Has.StatesProperty, new StatesCollection([.. this]));
                break;
            // we can safely ignore.
            case NotifyCollectionChangedAction.Replace:
            case NotifyCollectionChangedAction.Move:
            default:
                break;
        }
    }
}
