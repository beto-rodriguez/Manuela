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

    public XamlCondition HasText { get; } =
        new(visualElement => ((Entry)visualElement).Text?.Length > 5);

    public XamlCondition IsFocused { get; } =
        new(visualElement => visualElement.IsFocused);

    public XamlCondition IsValid2 { get; } =
        new(visualElement => IsValid);

    public XamlCondition IsThisThing { get; } =
        new(visualElement => ThisThing.MyProperty > 0);
}


public class ThisThing : INotifyPropertyChanged
{
    public int MyProperty { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}
