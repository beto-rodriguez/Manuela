using Microsoft.Maui.Platform;
using Manuela.Theming;

#if ANDROID
using AndroidX.Core.View;
#endif

namespace Manuela.WindowStyle;

public class ManuelaWindow
{
    public static void SetPointerPassthroughRegion(Region[] regions, Window[]? windows = null)
    {
#if WINDOWS
        windows ??= Application.Current?.Windows.ToArray() ?? [];

        foreach (var window in windows)
        {
            if (window.Handler.PlatformView is not Microsoft.UI.Xaml.Window w) continue;

            var nonClientInputSrc = Microsoft.UI.Input.InputNonClientPointerSource.GetForWindowId(w.AppWindow.Id);

            nonClientInputSrc.SetRegionRects(
                Microsoft.UI.Input.NonClientRegionKind.Passthrough,
                regions.Select(x => x.ToRectInt32()).ToArray());
        }
#endif
    }

    public static void SetWindowColors(Color topColor, Color bottomColor)
    {
#if ANDROID
        if (!OperatingSystem.IsAndroidVersionAtLeast(23)) return;

        var activity = Platform.CurrentActivity;
        var window = activity?.Window
            ?? throw new Exception("Unable to get Android Activity.");

        window.SetStatusBarColor(topColor.ToPlatform());

        var isTopLight = topColor.Red * .2126 + topColor.Green * .7152 + topColor.Blue * .0722 > 0.5;
        WindowCompat.GetInsetsController(window, window.DecorView).AppearanceLightStatusBars = isTopLight;

        if (!OperatingSystem.IsAndroidVersionAtLeast(30)) return;

        window.SetNavigationBarColor(new Android.Graphics.Color(
           (byte)(255 * bottomColor.Red),
           (byte)(255 * bottomColor.Green),
           (byte)(255 * bottomColor.Blue)));

        var wic = window.DecorView?.WindowInsetsController
            ?? throw new Exception("Unable to get Android WindowInsetsController");

        //https://developer.android.com/reference/android/view/WindowInsetsController#APPEARANCE_LIGHT_NAVIGATION_BARS
        const int APPEARANCE_LIGHT_NAVIGATION_BARS = 0x00000010;

        var isBottomLight = bottomColor.Red * .2126 + bottomColor.Green * .7152 + bottomColor.Blue * .0722 > 0.5;
        if (!isBottomLight) return;

        wic.SetSystemBarsAppearance(
            APPEARANCE_LIGHT_NAVIGATION_BARS, APPEARANCE_LIGHT_NAVIGATION_BARS);
#endif
    }

}
