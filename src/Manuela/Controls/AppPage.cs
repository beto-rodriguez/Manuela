// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.


using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls.Internals;

namespace Manuela;

public class AppPage : ContentPage
{
    private static AppPage? s_current;
    private AppBody? _appBodyElement;

    public AppPage()
    {
        Current = this;
        Loaded += OnAppLoaded;
    }

    public static AppPage Current
    {
        get => s_current ?? throw new Exception($"An {nameof(AppPage)} instance is not initialized yet.");
        private set => s_current = value;
    }

    public View? Body
    {
        set
        {
            if (_appBodyElement is null) GetAppBody();
            _appBodyElement!.Content = value;
        }
    }

    protected virtual void OnAppLoaded(object? sender, EventArgs e)
    { }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName != nameof(Content)) return;
        GetAppBody();

#if WINDOWS
        Content.Margin = new(-1, -32, 0, -1);
#endif

#if MACCATALYST
        Content.Margin = new(-1, -73, 0, -1);
        SizeChanged += (s, e) =>
        {
            // scene.FullScreen is only available on 16.0 and later
            // it means that full screen is not displayed properly on earlier versions
            // unless we find a way to get the full screen status for those versions.

            if (!OperatingSystem.IsMacCatalystVersionAtLeast(16)) return;

            var window = (UIKit.UIWindow?)Window.Handler.PlatformView;
            var scene = window?.WindowScene;
            if (scene is null) return;

            Content.Margin = scene.FullScreen
                ? new(0)
                : new(-1, -73, 0, -1);
        };
#endif
    }

    private void GetAppBody()
    {
        TryGetAppBody(Content);

        if (_appBodyElement is null)
            throw new InvalidOperationException(
                $"{nameof(AppBody)} not found. Manuela required an element of type {nameof(AppBody)}, " +
                $"to get more info and get started, see Manuela docs.");
    }

    private void TryGetAppBody(IView view)
    {
        if (view is AppBody appBody)
        {
            _appBodyElement = appBody;
            return;
        }
        else if (view is IContentView contentView && contentView.Content is IView contentViewContent)
        {
            TryGetAppBody(contentViewContent);
        }
        else if (view is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                TryGetAppBody(child);
                if (_appBodyElement is not null) return;
            }
        }
    }
}
