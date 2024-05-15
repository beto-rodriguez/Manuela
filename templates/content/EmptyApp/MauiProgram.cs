using EmptyApp.Views;
using Manuela;
using Manuela.AppRouting;
using Microsoft.Extensions.Logging;

namespace EmptyApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseManuela(
            routes: [
                new Route<Home>(),
                new Route<Second>()
            ]);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
