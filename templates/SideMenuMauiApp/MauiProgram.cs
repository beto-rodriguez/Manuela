using Manuela;
using MauiIcons.SegoeFluent;
using Microsoft.Extensions.Logging;
using SideMenuMauiApp.Layout;
using SideMenuMauiApp.Views;

namespace SideMenuMauiApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseManuela(
                AppRoutes.Build(routes => routes
                    .Add<MainView>(SegoeFluentIcons.Home, "Home")
                    .Add<AnotherView>(SegoeFluentIcons.ViewDashboard, "More")
                    .Add<Settings>(SegoeFluentIcons.Settings, "Settings")
                    // Hidden routes are not visible in the app menu, but are accessible by navigating to the route.
                    // .AddHidden<AnotherView>()
                    ))
            .UseSegoeFluentMauiIcons()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
