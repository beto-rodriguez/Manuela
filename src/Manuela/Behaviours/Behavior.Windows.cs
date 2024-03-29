#if WINDOWS

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Manuela.Behaviors;

/// <summary>
/// A class that adds platform-specific events to the chart.
/// </summary>
public partial class Behavior
{
    private void OnWindowsPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not UIElement uiElement) return;

        _ = uiElement.CapturePointer(e.Pointer);
        InvokeDown();
    }

    private void OnWindowsPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not UIElement uiElement) return;

        uiElement.ReleasePointerCapture(e.Pointer);
        InvokeUp();
    }
}

#endif
