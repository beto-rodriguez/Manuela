using Microsoft.Maui.Handlers;

namespace Manuela;

public static class ManuelaExtensions
{
    public static MauiAppBuilder UseManuela(
        this MauiAppBuilder builder,
        Route[]? routes = null,
        bool addBorderlessPicker = true)
    {
        routes ??= [];
        var serviceCollection = builder.Services;

        foreach (var route in routes)
        {
            Routing.Routes.Add(route.Name, route);

            if (route.IsSingleton)
                _ = serviceCollection.AddSingleton(route.ViewType);
            else
                _ = serviceCollection.AddTransient(route.ViewType);
        }

        Routing.ServiceCollection = serviceCollection;

        if (addBorderlessPicker)
        {
            PickerHandler.Mapper.AppendToMapping("borderless", (handler, picker) =>
            {
#if ANDROID
            handler.PlatformView.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(
                    Microsoft.Maui.Controls.Compatibility.Platform.Android.ColorExtensions.ToAndroid(Colors.Transparent));
#elif IOS && !MACCATALYST
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif MACCATALYST
        // how?
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Style = null;
#endif
            });
        }

        return builder;
    }
}
