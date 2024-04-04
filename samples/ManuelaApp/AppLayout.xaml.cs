using Manuela;
using MauiIcons.Core;

namespace ManuelaApp;

public partial class AppLayout : AppPage
{
	public AppLayout()
	{
		InitializeComponent();

        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();
    }
}
