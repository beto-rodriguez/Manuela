using Manuela;
using Manuela.AppRouting;
using MauiIcons.Material.Rounded;
using Microsoft.Extensions.Logging;
using SideMenuMauiApp.Views;

namespace SideMenuMauiApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseManuela([
                new Route<MainView>(),
                new Route<AnotherView>(),
                new Route<Settings>()
            ])
            .UseMaterialRoundedMauiIcons()
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
