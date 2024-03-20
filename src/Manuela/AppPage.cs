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
            if (_appBodyElement is null) FindAppContent();
            _appBodyElement!.Content = value;
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName != nameof(Content)) return;
        FindAppContent();

#if WINDOWS
        // a hack to cover also the title bar (32px)
        Content.Margin = new(-1, -32, 0, -1);
#endif
    }

    private void FindAppContent(View? parent = null)
    {
        parent ??= Content;

        if (parent is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                if (child is AppBody appBoddy)
                {
                    _appBodyElement = appBoddy;
                    return;
                }

                if (child is Layout cLayout) FindAppContent(cLayout);
                if (_appBodyElement is not null) return;
            }
        }

        if (_appBodyElement is null)
            throw new InvalidOperationException($"The {nameof(Body)} element was not found.");
    }
}
