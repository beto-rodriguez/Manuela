#if ANDROID

using Android.Views;

namespace Manuela.Behaviors;

/// <summary>
/// A class that adds platform-specific events to the chart.
/// </summary>
public partial class Behavior
{
    private void OnAndroidTouched(object? sender, Android.Views.View.TouchEventArgs e)
    {
        var viewGroup = (ViewGroup?)sender;
        if (e.Event is null || viewGroup is null) return;

#pragma warning disable CA1416
        switch (e.Event.ActionMasked)
        {
            case MotionEventActions.ButtonPress:
            case MotionEventActions.Pointer1Down:
            case MotionEventActions.Pointer2Down:
            case MotionEventActions.Pointer3Down:
            case MotionEventActions.Down:
                InvokeDown();
                break;
            case MotionEventActions.ButtonRelease:
            case MotionEventActions.Pointer1Up:
            case MotionEventActions.Pointer2Up:
            case MotionEventActions.Pointer3Up:
            case MotionEventActions.Up:
            case MotionEventActions.Cancel:
                InvokeUp();
                break;
            case MotionEventActions.HoverEnter:
                InvokeEnter();
                break;
            case MotionEventActions.HoverExit:
                InvokeExit();
                break;
            case MotionEventActions.Move:
            case MotionEventActions.HoverMove:
            case MotionEventActions.Mask:
            case MotionEventActions.Outside:
            case MotionEventActions.PointerIdMask:
            case MotionEventActions.PointerIdShift:
            default:
                break;
        }
#pragma warning restore CA1416
    }
}

#endif
