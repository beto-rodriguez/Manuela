using Manuela.Forms;

namespace Manuela.Dialogs;

public static class DialogExtensions
{
    /// <summary>
    /// Sets the dialog response, the view must be a dialog.
    /// </summary>
    public static void SetDialogResponse(this View view, object? response)
    {
        var tsc = Has.GetModalTcs(view) ?? throw new InvalidOperationException("The view is not a dialog.");
        tsc.Respond(response);
    }

    /// <summary>
    /// Cancels the dialog, the view must be a dialog.
    /// </summary>
    public static void CancelDialog(this View view)
    {
        var tsc = Has.GetModalTcs(view) ?? throw new InvalidOperationException("The view is not a dialog.");
        tsc.Cancel();
    }
}
