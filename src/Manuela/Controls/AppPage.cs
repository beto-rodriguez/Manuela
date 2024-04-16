// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.


using System.Diagnostics;
using System.Runtime.CompilerServices;

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
            var c = DeviceDisplay.Current.MainDisplayInfo;
            var d = c.Density;
            var w = c.Width / d;
            var h = c.Height / d;

            Trace.WriteLine($"{c.Width}x{c.Height} / {d} = {w}x{h}");
            Trace.WriteLine($"page: {Width}x{Height}, {Width / w:N2}x{Height / h:N2}");


            //// if full screen, remode the negative margin
            //if (wi && h == Height)
            //    Content.Margin = new(0);
            //else
            //    Content.Margin = new(-1, -73, 0, -1);
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
