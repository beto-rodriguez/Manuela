using MauiIcons.Core;

namespace ManuelaApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();
    }
}
