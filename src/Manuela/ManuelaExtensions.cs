﻿using Manuela.AppRouting;
using Manuela.Theming;
using Manuela.Things;


#if MACCATALYST
using Microsoft.Maui.LifecycleEvents;
using UIKit;
#endif

namespace Manuela;

public static class ManuelaExtensions
{
    public static MauiAppBuilder UseManuela(
        this MauiAppBuilder builder,
        Route[]? routes = null,
        Theme? theme = null)
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

            if (route.ViewModelType is not null)
                _ = serviceCollection.AddTransient(route.ViewModelType);
        }

        ManuelaThings.ServiceCollection = serviceCollection;

        Theme.Current = theme ?? new();

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

        return builder;
    }
}
