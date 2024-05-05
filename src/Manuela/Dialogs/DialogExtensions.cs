namespace Manuela.Dialogs;

public static class DialogExtensions
{
    /// <summary>
    /// Sets the dialog response, the view must be a dialog.
    /// </summary>
    /// <param name="view">The dialog view shown.</param>
    public static void SetDialogResponse(this View view, object? response)
    {
        var tsc = Has.GetModalTcs(view) ?? throw new InvalidOperationException("The view is not a dialog.");

        // already completed, probably the user clicked twice while the dialog was closing
        if (tsc.Task.Status != TaskStatus.WaitingForActivation) return;

        tsc.SetResult(response);
    }

    /// <summary>
    /// Cancels the dialog, the view must be a dialog.
    /// </summary>
    /// <param name="view">the view.</param>
    public static void CancelDialog(this View view)
    {
        var tsc = Has.GetModalTcs(view) ?? throw new InvalidOperationException("The view is not a dialog.");

        // already completed, probably the user clicked twice while the dialog was closing
        if (tsc.Task.Status != TaskStatus.WaitingForActivation) return;

        tsc.SetCanceled();
    }
}
