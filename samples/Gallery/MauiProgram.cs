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
                new Route<Brushes>(isSingleton: true),
                new Route<States>(isSingleton: true),
                new Route<Sizing>(isSingleton: true),
                new Route<Spacing>(isSingleton: true),
                new Route<Transitions>(isSingleton: true),
                new Route<Forms>(isSingleton: true),
                new Route<Validation>(isSingleton: true),
                new Route<Dialogs>(isSingleton: true)
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
