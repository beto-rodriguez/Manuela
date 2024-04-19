using Microsoft.Maui.Platform;

#if ANDROID
using AndroidX.Core.View;
#endif

#if IOS
using ObjCRuntime;
using UIKit;
using Foundation;
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

    // based on Maui Community Toolkit approach.

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

#if IOS
        var uIColor = topColor.ToPlatform();

        if (OperatingSystem.IsIOSVersionAtLeast(13))
        {
            var statusBarTag = new IntPtr(38482);

            //var windows = UIApplication.SharedApplication.Windows;

            var windows = Application.Current?.Windows
                .Select(x => x.Handler.PlatformView)
                .Cast<UIWindow>()
                .ToArray()
                ?? [];

            foreach (var uIWindow in windows)
            {
                var uIView = uIWindow.ViewWithTag(statusBarTag);

                // suppressed because it is not reachable? the if clause must prevent that?

#pragma warning disable CA1416 // Validate platform compatibility
                var cGRect = uIWindow.WindowScene?.StatusBarManager?.StatusBarFrame;
#pragma warning restore CA1416 // Validate platform compatibility

                if (!cGRect.HasValue) continue;
                uIView ??= new UIView(cGRect.Value);

                uIView.Tag = statusBarTag;
                uIView.BackgroundColor = uIColor;
                uIView.TintColor = uIColor;
#pragma warning disable CA1422 // Validate platform compatibility
                uIView.Frame = UIApplication.SharedApplication.StatusBarFrame;
#pragma warning restore CA1422 // Validate platform compatibility
                foreach (var item in uIWindow.Subviews.Where((UIView x) => x.Tag == statusBarTag).ToList())
                {
                    item.RemoveFromSuperview();
                }

                uIWindow.AddSubview(uIView);
                UpdateStatusBarAppearance(uIWindow);
            }
        }
        else
        {
            if (
                UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) is UIView uIView2 &&
                uIView2.RespondsToSelector(new Selector("setBackgroundColor:"))
                )
            {
                uIView2.BackgroundColor = uIColor;
            }

            UpdateStatusBarAppearance();
        }

        var isLight = topColor.Red * .2126 + topColor.Green * .7152 + topColor.Blue * .0722 > 0.5;

        var style = isLight ? UIStatusBarStyle.BlackTranslucent : UIStatusBarStyle.DarkContent;
        UIApplication.SharedApplication.SetStatusBarStyle(style, animated: false);

        UpdateStatusBarAppearance();
#endif
    }

#if IOS
    private static void UpdateStatusBarAppearance(UIWindow? window)
    {
        var uIViewController = window?.RootViewController
            ?? WindowStateManager.Default.GetCurrentUIViewController()
            ?? throw new InvalidOperationException("RootViewController cannot be null");

        while (uIViewController.PresentedViewController != null)
        {
            uIViewController = uIViewController.PresentedViewController;
        }

        uIViewController.SetNeedsStatusBarAppearanceUpdate();
    }

    private static void UpdateStatusBarAppearance()
    {
        if (OperatingSystem.IsIOSVersionAtLeast(13))
        {
            //var windows = UIApplication.SharedApplication.Windows;

            var windows = Application.Current?.Windows
                .Select(x => x.Handler.PlatformView)
                .Cast<UIWindow>()
                .ToArray()
                ?? [];

            for (var i = 0; i < windows.Length; i++)
            {
                UpdateStatusBarAppearance(windows[i]);
            }
        }
        else
        {
            UpdateStatusBarAppearance(UIApplication.SharedApplication.KeyWindow);
        }
    }
#endif

}
