using Manuela.AppRouting;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.LifecycleEvents;

#if MACCATALYST
using UIKit;
#endif

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
            AppRouting.Routing.Routes.Add(route.RouteName, route);

            if (route.IsSingleton)
                _ = serviceCollection.AddSingleton(route.ViewType);
            else
                _ = serviceCollection.AddTransient(route.ViewType);
        }

        AppRouting.Routing.ServiceCollection = serviceCollection;

#if MACCATALYST
        builder.ConfigureLifecycleEvents(lifecycle =>
        {
            lifecycle.AddiOS(ios =>
            {
                ios.FinishedLaunching((app, options) =>
                {
                    var titleBar = app.Delegate.GetWindow().WindowScene?.Titlebar;
                    if (titleBar == null) return true;

                    titleBar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
                    titleBar.Toolbar = null;

                    return true;
                });
            });
        });
#endif

#if WINDOWS
        // MauiAppTitleBarTemplate is an obstacle for the SetPointerPassthroughRegion method.
        static object GetTemplate() => Microsoft.UI.Xaml.Markup.XamlReader.Load(@"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><StackPanel Height=""0"" /></DataTemplate>");
        MauiWinUIApplication.Current.Resources.Add("MauiAppTitleBarTemplate", GetTemplate());
#endif

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
