using Manuela.States.Screen;
using Manuela.Styling;

namespace ManuelaAppTemplate.AppLayout;

public partial class AppCollapsedMenu : Border
{
    private bool _isMenuOpen = false;
    private bool? _isLargeScreen;
    private DateTime _loadTime;

    public AppCollapsedMenu()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            foreach (var item in Options.Children.OfType<MoreMenuItem>())
            {
                item.Tapped += Item_Tapped;
            }
            _loadTime = DateTime.Now;
        };

        SizeChanged += (_, _) =>
        {
            var isLargeScreen = this.GetScreenBreakpoint() >= Breakpoint.Md;
            if (_isLargeScreen == isLargeScreen)
            {
                // when the loaded event is called, the size of this control is not yet set
                // when is it safe to call this method?

                // for now, we will update the menu while this call is made within 500ms of the loaded event
                if ((DateTime.Now - _loadTime).TotalMilliseconds < 500)
                {
                    MoveMenu(true);
                }

                return;
            }

            _isLargeScreen = isLargeScreen;
            MoveMenu(false);
        };
    }

    public bool IsOpen => _isMenuOpen;

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
        var isVertical =
            this.GetScreenBreakpoint() >= Breakpoint.Md &&
            !(DeviceInfo.Idiom == DeviceIdiom.Phone || DeviceInfo.Idiom == DeviceIdiom.Tablet);

        if (isVertical)
        {
            TranslationY = 0;
            this.SetManuelaProperty(ManuelaProperty.TranslateX, _isMenuOpen ? 0d : -Width, null, animated);
        }
        else
        {
            TranslationX = 0;
            this.SetManuelaProperty(ManuelaProperty.TranslateY, _isMenuOpen ? 0d : Height, null, animated);
        }
    }

    private void Item_Tapped(HorizontalStackLayout obj)
    {
        Close();
    }
}
