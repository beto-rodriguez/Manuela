using Gallery.Views;
using Manuela;
using Manuela.AppRouting;
using Manuela.Styling;
using Manuela.Things;
using MauiIcons.Core;
using MauiIcons.SegoeFluent;
using Microsoft.Extensions.Logging;

namespace Gallery;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseManuela([
                new Route<Brushes>(),
                new Route<States>(),
                new Route<Sizing>(),
                new Route<Spacing>(),
                new Route<Transitions>(),
                new Route<Forms>(),
                new Route<Validation>(),
                new Route<Dialogs>()
            ])
            .UseSegoeFluentMauiIcons()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("JetBrainsMono-VariableFont_wght.ttf", "JetBrainsMono");
            });

        ManuelaThings.RegisterType<MauiIcon>(ManuelaProperty.TextColor, MauiIcon.IconColorProperty);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
