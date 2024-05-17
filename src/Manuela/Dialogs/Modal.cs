using Manuela.Styling;
using Microsoft.Maui.Layouts;

namespace Manuela.Dialogs;

public static class Modal
{
    private static readonly string[] s_modalShadowStyle = ["modal-shadow"];
    private static readonly string[] s_modalWindowStyle = ["modal-window"];

    private static readonly Stack<View> s_dialogStack = [];
    private static Border? s_shadow;
    private static AbsoluteLayout? s_root;

    private static AbsoluteLayout Root => s_root ??= AppPage.Current.Content as AbsoluteLayout
        ?? throw new("The root of the page is not an AbsoluteLayout, that is a requirement to show dialogs.");

    public static Task<ModalOptions> Show(
        string? title, string? message, ModalOptions answerType, DialogSize size = DialogSize.Small, bool animated = true)
    {
        var dialog = new DefaultDialog();
        dialog.SetContent(title, message, answerType);

        return Show<ModalOptions>(dialog, size, animated);
    }

    public static async Task<T?> Show<T>(View view, DialogSize size = DialogSize.Medium, bool animated = true)
    {
        var showDialogTask = Show(view, size, animated);

        T? answer;

        try
        {
            answer = (T?)await showDialogTask;
        }
        catch (OperationCanceledException)
        {
            answer = default;
        }
        catch (Exception)
        {
            answer = default;
        }
        finally
        {
            _ = s_dialogStack.Pop();

            var window = (Border)view.Parent;
            var windowWrap = (ScrollView)window.Parent;

            if (animated)
            {
                AnimateStack(true);

                window.SetManuelaProperty(
                    ManuelaProperty.TranslateY,
                    -view.Height - 90,
                    new() { Easing = Easing.SpringIn, Duration = 300 });

                window.SetManuelaProperty(
                    ManuelaProperty.Scale,
                    0.9d,
                    new() { Easing = Easing.SpringIn, Duration = 300 });
            }

            if (s_dialogStack.Count == 0)
            {
                _ = Root.Children.Remove(s_shadow);
                s_shadow = null;
            }
            else
            {
                windowWrap.IsVisible = false;
            }

            // remove the window after the animation is done
            _ = Task.Delay(300)
                .ContinueWith(t => Root.Dispatcher
                    .Dispatch(() => _ = Root.Children.Remove(windowWrap)));
        }

        return answer;
    }

    private static Task<object?> Show(View view, DialogSize size = DialogSize.Small, bool animated = true)
    {
        if (s_shadow is null) Root.Children.Add(s_shadow = new Border { StyleClass = s_modalShadowStyle });

        var window = new Border
        {
            VerticalOptions = LayoutOptions.Start,
            StyleClass = s_modalWindowStyle,
            MaximumWidthRequest = size switch
            {
                DialogSize.Small => 400,
                DialogSize.Medium => 800,
                DialogSize.Large => 1200,
                _ => 800
            },
            Content = view
        };

        var windowWrap = new ScrollView
        {
            ZIndex = 100,
            Background = null,
            Content = window
        };

        AbsoluteLayout.SetLayoutFlags(windowWrap, AbsoluteLayoutFlags.SizeProportional);
        AbsoluteLayout.SetLayoutBounds(windowWrap, new Rect(0, 0, 1, 1));

        if (animated)
        {
            window.Scale = 0.8;
            window.TranslationY = 60;
            window.SetManuelaProperty(
                ManuelaProperty.TranslateY,
                0d,
                new() { Easing = Easing.SpringOut, Duration = 300 });
            window.SetManuelaProperty(
                ManuelaProperty.Scale,
                1d,
                new() { Easing = Easing.SpringOut, Duration = 300 });
        }

        Root.Children.Add(windowWrap);

        s_dialogStack.Push(view);
        AnimateStack(false);

        var tcs = new TaskCompletionSource<object?>();
        Has.SetModalTcs(view, tcs);

        return tcs.Task;
    }

    private static void AnimateStack(bool isRemove)
    {
        var i = 0;

        foreach (var dialog in s_dialogStack)
        {
            i++;

            // skip the first element when it was just added
            // because it will be animated in the Show method
            if (i == 1 && !isRemove) continue;

            var w = (Border)dialog.Parent;
            w.SetManuelaProperty(
                ManuelaProperty.TranslateY,
                -(i - 1) * 20d,
                new() { Easing = Easing.CubicOut, Duration = 300 });
            w.SetManuelaProperty(
                ManuelaProperty.Scale,
                1 - (i - 1) * 0.1,
                new() { Easing = Easing.CubicOut, Duration = 300 });
        }
    }
}
