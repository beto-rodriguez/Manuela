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
            Router.Current.Routes.Add(route);

            _ = route.IsSingleton
                ? serviceCollection.AddSingleton(route.ViewType)
                : serviceCollection.AddTransient(route.ViewType);

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

    public static Point GetElementCoordinates(this IView view)
    {
        if (view is not VisualElement element) return new Point();

#if WINDOWS
        var window = (Microsoft.UI.Xaml.Window?)AppPage.Current.Window?.Handler.PlatformView;
        var platformview = (Microsoft.UI.Xaml.UIElement?)element.Handler?.PlatformView;

        if (window is null || platformview is null) return new Point();

        var point = platformview
            .TransformToVisual(window.Content)
            .TransformPoint(new Windows.Foundation.Point(0, 0));

        return new Point(point.X, point.Y);
#endif

        return new Point();
    }

    public static T? FindChildOfType<T>(this IView view)
    {
        if (view is T tElement)
        {
            return tElement;
        }
        else if (view is IContentView contentView && contentView.Content is IView contentViewContent)
        {
            return FindChildOfType<T>(contentViewContent);
        }
        else if (view is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                var childElement = FindChildOfType<T>(child);
                if (childElement is not null) return childElement;
            }
        }

        return default;
    }
}
