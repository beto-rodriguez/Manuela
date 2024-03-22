using System.ComponentModel;
using Manuela.Expressions;

namespace MauiApp1;

public partial class App : Application
{
    public static bool IsValid { get; set; }
    public static ThisThing ThisThing { get; set; } = new ThisThing();

    public App()
    {
        BindingContext = this;
        InitializeComponent();
        InitializeTriggers();
    }

    public XamlCondition IsVisualFocused { get; } =
        new(visualElement => visualElement.IsFocused);

    public XamlCondition IsVisualDisabled { get; } =
        new(visualElement =>
        {
            return !visualElement.IsEnabled;
        });
}

public class ThisThing : INotifyPropertyChanged
{
    public int MyProperty { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}
