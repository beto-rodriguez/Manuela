using Manuela;

namespace MauiApp1;

public partial class App : Application
{
    public App()
    {
        BindingContext = this;
        InitializeComponent();
    }

    public Manuela.Condition IsVisualFocused => new()
    {
        Predicate = v => v.IsFocused,
        Triggers = v =>
        [
            new ConditionUpdateTrigger(v, [nameof(VisualElement.IsFocused)])
        ]
    };
}
