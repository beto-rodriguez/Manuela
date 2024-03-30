namespace Manuela.Behaviors;

// a custom behavior that provides platform-specific events to fire the Pressed conditional style.

// this is an alternative to the TapGesture/PointerGesture recognizers...
// this class provides the following events:

//  -Down:          called when the pointer/tap goes Down.
//  -Up:            called when the pointer/tap goes Up.

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

    private void InvokeDown()
    {
        Down?.Invoke();
    }

    private void InvokeUp()
    {
        Up?.Invoke();
    }

    public void Dispose()
    {
        if (_visual is null) return;

#if ANDROID
        var contentViewGroup = (Microsoft.Maui.Platform.ContentViewGroup?)_visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentViewGroup");

        contentViewGroup.Touch -= OnAndroidTouched;
#endif

#if MACCATALYST || IOS
        var contentView = (Microsoft.Maui.Platform.ContentView?)_visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentView");

        if (_longPressRecognizer is not null)
            contentView.RemoveGestureRecognizer(_longPressRecognizer);
#endif

#if WINDOWS
        var contentPanel = (Microsoft.UI.Xaml.UIElement?)_visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentPanel");

        contentPanel.PointerPressed -= OnWindowsPointerPressed;
        contentPanel.PointerReleased -= OnWindowsPointerReleased;
#endif

        _visual = null;
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
        var contentView = (Microsoft.Maui.Platform.ContentView?)visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentView");

        contentView.UserInteractionEnabled = true;
        contentView.AddGestureRecognizer(GetMacCatalystLongPress(contentView));
#endif

#if WINDOWS
        var contentPanel = (Microsoft.UI.Xaml.UIElement?)visual.Handler?.PlatformView
            ?? throw new Exception("Unable to cast to ContentPanel");

        contentPanel.PointerPressed += OnWindowsPointerPressed;
        contentPanel.PointerReleased += OnWindowsPointerReleased;
#endif
    }
}
