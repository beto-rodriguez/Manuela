namespace MauiApp1;

public partial class App : Application
{
    public App()
    {
        BindingContext = this;
        InitializeComponent();

        Data = Enumerable.Range(1, 100).Select(i => $"Item {i}").ToArray();
    }

    public string[] Data { get; set; }
}
