#if MACCATALYST || IOS

using UIKit;

namespace Manuela.Behaviors;

/// <summary>
/// A class that adds platform-specific events to the chart.
/// </summary>
public partial class Behavior
{
    private UILongPressGestureRecognizer? _longPressRecognizer;

    private UILongPressGestureRecognizer GetMacCatalystLongPress(UIView view)
    {
        return _longPressRecognizer = new UILongPressGestureRecognizer((UILongPressGestureRecognizer e) =>
        {
            var location = e.LocationInView(view);

            switch (e.State)
            {
                case UIGestureRecognizerState.Began:
                    InvokeDown();
                    break;
                case UIGestureRecognizerState.Changed:
                    // ignore here.
                    break;
                case UIGestureRecognizerState.Cancelled:
                case UIGestureRecognizerState.Ended:
                    InvokeUp();
                    break;
                case UIGestureRecognizerState.Possible:
                case UIGestureRecognizerState.Failed:
                default:
                    break;
            }
        })
        {
            MinimumPressDuration = 0,
            ShouldRecognizeSimultaneously = (g1, g2) => true
        };
    }
}

#endif
