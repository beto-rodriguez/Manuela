// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.


using System.Runtime.CompilerServices;

namespace Manuela;

public class AppPage : ContentPage
{
    private static AppPage? s_current;
    private AppBody? _appBodyElement;

    public AppPage()
    {
        Current = this;
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

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName != nameof(Content)) return;
        GetAppBody();

#if WINDOWS
        // a hack to cover also the title bar (32px)
        Content.Margin = new(-1, -32, 0, -1);
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
