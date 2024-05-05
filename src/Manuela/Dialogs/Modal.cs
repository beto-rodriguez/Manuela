using Manuela.Styling;

namespace Manuela.Dialogs;

public static class Modal
{
    private static readonly string[] s_modalShadowStyle = ["modal-shadow"];
    private static readonly string[] s_modalWindowStyle = ["modal-window"];

    private static Border? s_shadow;
    private static int dialogCount;
    private static AbsoluteLayout? s_root;

    private static AbsoluteLayout Root => s_root ??= AppPage.Current.Content as AbsoluteLayout
        ?? throw new("The root of the page is not an AbsoluteLayout, that is a requirement to show dialogs.");

    public static Task<Answer> Show(string? title, string? message, Answer answerType)
    {
        var dialog = new DefaultDialog();
        dialog.SetContent(title, message, answerType);

        return Show<Answer>(dialog);
    }

    public static async Task<T?> Show<T>(View view)
    {
        var showDialogTask = Show(view);

        T? answer;

        try
        {
            answer = (T)await showDialogTask;
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
            dialogCount--;
            if (dialogCount == 0)
            {
                _ = Root.Children.Remove(s_shadow);
                s_shadow = null;
            }

            // animate remove?
            _ = Root.Children.Remove((IView)view.Parent);
        }

        return answer;
    }

    public static Task<object> Show(View view)
    {
        if (s_shadow is null) Root.Children.Add(s_shadow = new Border { StyleClass = s_modalShadowStyle });

        var window = new Border
        {
            HorizontalOptions = LayoutOptions.Center,
            StyleClass = s_modalWindowStyle,
            Content = view
        };

        Has.SetTransitions(window, [
            new Transition { Property = ManuelaProperty.TranslateY, Easing = Easing.SpringOut, Duration = 500 }
        ]);

        window.TranslationY = 100;
        window.SetManuelaProperty(ManuelaProperty.TranslateY, 0d);

        Root.Children.Add(window);

        var tcs = new TaskCompletionSource<object>();
        Has.SetModalTcs(view, tcs);

        dialogCount++;

        return tcs.Task;
    }
}
