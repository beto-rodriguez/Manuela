namespace Manuela.Behaviors;

// a custom behavior that provides platform-specific events.
// this class is necessary to prevent a possible Maui issue where mixing TapGestures and PointerGestures
// causes strange issues (like invoking gestures twice).

//  -Down:          called when the pointer/tap goes Down.
//  -Up:            called when the pointer/tap goes Up.
//  -Enter:         called when the pointer Enters the control.
//  -Exit:          called when the pointer Exits the control.

// this is based on the LiveCharts2 project:
// https://github.com/beto-rodriguez/LiveCharts2/tree/master/src/LiveChartsCore.Behaviours

public partial class Behavior
{
    private VisualElement? _visual;

    public Behavior(VisualElement visual)
    {
        if (visual.Handler is null)
        {
            visual.HandlerChanged += (_, _) => Initialize(visual);
        }
        else
        {
            Initialize(visual);
        }
    }

    /// <summary>
    /// Called when the pointer/tap goes Down.
    /// </summary>
    public event Action? Down;

    /// <summary>
    /// Called when the pointer/tap goes Up.
    /// </summary>
    public event Action? Up;

    /// <summary>
    /// Called when the pointer Enters.
    /// </summary>
    public event Action? Enter;

    /// <summary>
    /// Called when the pointer Exits.
    /// </summary>
    public event Action? Exit;

    private void InvokeDown()
    {
        Down?.Invoke();
    }

    private void InvokeUp()
    {
        Up?.Invoke();
    }

    private void InvokeEnter()
    {
        Enter?.Invoke();
    }

    private void InvokeExit()
    {
        Exit?.Invoke();
    }

    private void Initialize(VisualElement visual)
    {
        _visual = visual;

#if ANDROID
        var contentViewGroup = (Android.Views.View?)visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentViewGroup");

        contentViewGroup.Touch += OnAndroidTouched;
#endif

#if MACCATALYST || IOS
        var uiView = (UIKit.UIView?)visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentView");

        uiView.UserInteractionEnabled = true;

#if MACCATALYST
        uiView.AddGestureRecognizer(GetMacCatalystHover(uiView));
#endif

        uiView.AddGestureRecognizer(GetMacCatalystLongPress(uiView));
#endif

#if WINDOWS
        var element = (Microsoft.UI.Xaml.UIElement?)visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentPanel");

        element.AddHandler(
            Microsoft.UI.Xaml.UIElement.PointerPressedEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerPressed),
            true);
        element.AddHandler(
            Microsoft.UI.Xaml.UIElement.PointerReleasedEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerReleased),
            true);
        element.AddHandler(
            Microsoft.UI.Xaml.UIElement.PointerEnteredEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerEntered),
            true);
        element.AddHandler(
            Microsoft.UI.Xaml.UIElement.PointerExitedEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerExited),
            true);
#endif
    }

    public void Dispose()
    {
        if (_visual is null) return;

#if ANDROID
        var contentViewGroup = (Android.Views.View?)_visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentViewGroup");

        contentViewGroup.Touch -= OnAndroidTouched;
#endif

#if MACCATALYST || IOS
        var uiView = (UIKit.UIView?)_visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentView");

#if MACCATALYST
        if (_hoverRecognizer is not null)
            uiView.RemoveGestureRecognizer(_hoverRecognizer);
#endif

        if (_longPressRecognizer is not null)
            uiView.RemoveGestureRecognizer(_longPressRecognizer);
#endif

#if WINDOWS
        var contentPanel = (Microsoft.UI.Xaml.UIElement?)_visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentPanel");

        contentPanel.RemoveHandler(
            Microsoft.UI.Xaml.UIElement.PointerPressedEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerPressed));
        contentPanel.RemoveHandler(
            Microsoft.UI.Xaml.UIElement.PointerReleasedEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerReleased));
        contentPanel.RemoveHandler(
            Microsoft.UI.Xaml.UIElement.PointerEnteredEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerEntered));
        contentPanel.RemoveHandler(
            Microsoft.UI.Xaml.UIElement.PointerExitedEvent,
            new Microsoft.UI.Xaml.Input.PointerEventHandler(OnWindowsPointerExited));
#endif

        _visual = null;
    }
}
