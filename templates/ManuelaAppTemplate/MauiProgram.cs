using Manuela;
using ManuelaAppTemplate.AppLayout;
using ManuelaAppTemplate.AppViews;
using MauiIcons.SegoeFluent;
using Microsoft.Extensions.Logging;

namespace ManuelaAppTemplate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseSegoeFluentMauiIcons()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseManuela(
                AppRoutes.Build(routes => routes
                    // Main routes are always visible in the app menu.
                    .AddMain<HomeView>(SegoeFluentIcons.Home, "Home")
                    .AddMain<SecondView>(SegoeFluentIcons.ViewDashboard, "Second")

                    // Secondary Routes are visible on medium or greater screens
                    .AddSecondary<ProfileView>(SegoeFluentIcons.ContactInfo, "My profile")
                    .AddSecondary<SettingsView>(SegoeFluentIcons.Settings, "Settings")

                    // Collapsed buttons add a button that is always visible to open the collapsed routes.
                    .AddCollapsed(SegoeFluentIcons.ExploreContent, "Explore", collapsed => collapsed
                        .Add<HomeView>(SegoeFluentIcons.Home, "Home Collapsed")
                        .Add<SecondView>(SegoeFluentIcons.ViewDashboard, "Second Collapsed")
                        .Add<AnotherView>(SegoeFluentIcons.Play, "Another"))

                    // In this exaple, we use the "mobile-and-small-visible" style class
                    // this class makes the item button visible only on small screens and mobile devices.
                    // its useful to show the Secondary menu items, because they are not visible on small screens.
                    .AddCollapsed(SegoeFluentIcons.More, "More", collapsed => collapsed
                        .Add<AnotherView>(
                            SegoeFluentIcons.Play, "Another")
                        .Add<ProfileView>(
                            SegoeFluentIcons.ContactInfo, "My profile", styleClass: ["mobile-and-small-visible"])
                        .Add<SettingsView>(
                            SegoeFluentIcons.Settings, "Settings", styleClass: ["mobile-and-small-visible"])
                    )

                    // Hidden routes are not visible in the app menu, but are accessible by navigating to the route.
                    // .AddHidden<AnotherView>()
                    ));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
