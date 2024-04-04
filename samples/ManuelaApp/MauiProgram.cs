using Manuela;
using Manuela.AppRouting;
using Manuela.Styling;
using Manuela.Things;
using ManuelaApp.Views;
using MauiIcons.Core;
using MauiIcons.Material.Rounded;
using Microsoft.Extensions.Logging;

namespace ManuelaApp;
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

        ManuelaThings.RegisterType<MauiIcon>(ManuelaProperty.TextColor, MauiIcon.IconColorProperty);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
