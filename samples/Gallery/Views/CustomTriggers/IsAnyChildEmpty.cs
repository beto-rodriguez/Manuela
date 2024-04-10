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
        Condition = new Manuela.Expressions.XamlCondition(v => true);
    }

    //protected void OnInitialized2()
    //{
    //    Condition = new(IsActive)
    //    {
    //        Triggers = visualElement =>
    //        {
    //            var hashSet = new HashSet<Trigger>();

    //            var dummyCondition = TriggersCondition(visualElement, hashSet);

    //            return
    //            [
    //                new(Layout                        , ["Children"]),
    //                new(x                             , ["Text"])
    //            ];
    //        }
    //    };
    //}

    //private bool TriggersCondition(VisualElement visualElement, Dictionary<INotifyPropertyChanged, Trigger> hashSet)
    //{
    //    return Layout.Children.OfType<Entry>().Any(x => IsNotifier(x, hashSet).Text?.Length == 0);
    //}

    //private static T IsNotifier<T>(
    //    T notifier,
    //    HashSet<Trigger> hashSet)
    //        where T : INotifyPropertyChanged
    //{
    //    if (hashSet.Contains(notifier)) return;

    //    _ = hashSet.Add(notifier);
    //    return notifier;
    //}
}
