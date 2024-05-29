namespace Manuela.Forms;

public static class DialogExtensions
{
    /// <summary>
    /// Sets the response of an attached TaskCompletionSource of a dialog.
    /// </summary>
    public static void Respond(this TaskCompletionSource<object?> tcs, object? response)
    {
        // already completed, probably the user clicked twice while the dialog was closing
        if (tcs.Task.Status != TaskStatus.WaitingForActivation) return;

        tcs.SetResult(response);
    }

    /// <summary>
    /// Cancels an attached TaskCompletionSource to a dialog.
    /// </summary>
    public static void Cancel(this TaskCompletionSource<object?> tcs)
    {
        if (tcs.Task.Status != TaskStatus.WaitingForActivation) return;
        tcs.SetCanceled();
    }
}
