using Manuela.States.Screen;
using Manuela.Styling;

namespace Gallery.Layout;

public partial class AppMenuMoreOptions : Border
{
    private bool _isMenuOpen = false;
    private bool? _isLargeScreen;

    public AppMenuMoreOptions()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            MoveMenu(false);
            foreach (var item in Options.Children.OfType<MoreMenuItem>())
            {
                item.Tapped += Item_Tapped;
            }
        };

        SizeChanged += (_, _) =>
        {
            var isLargeScreen = this.GetScreenBreakpoint() >= Breakpoint.Md;
            if (_isLargeScreen == isLargeScreen) return;

            _isLargeScreen = isLargeScreen;
            MoveMenu(false);
        };
    }

    public void Open(bool animated = true)
    {
        _isMenuOpen = true;
        MoveMenu(animated);
    }

    public void Close(bool animated = true)
    {
        _isMenuOpen = false;
        MoveMenu(animated);
    }

    public void Toggle(bool animated = true)
    {
        _isMenuOpen = !_isMenuOpen;
        MoveMenu(animated);
    }

    private void MoveMenu(bool animated)
    {
        if (this.GetScreenBreakpoint() >= Breakpoint.Md)
        {
            TranslationY = 0;
            this.SetManuelaProperty(ManuelaProperty.TranslateX, _isMenuOpen ? 0d : -300d, animated);
        }
        else
        {
            TranslationX = 0;
            this.SetManuelaProperty(ManuelaProperty.TranslateY, _isMenuOpen ? 0d : 500d, animated);
        }
    }

    private void Item_Tapped(HorizontalStackLayout obj)
    {
        Close();
    }
}
