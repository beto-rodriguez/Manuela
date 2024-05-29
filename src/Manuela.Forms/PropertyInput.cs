namespace Manuela.Forms;

public class PropertyInput(
    Form form,
    string propertyName,
    string displayName,
    Func<object?> getter,
    Action<object?> setter)
{
    /// <summary>
    /// Initializes the input control.
    /// </summary>
    /// <param name="inputControl"></param>
    public void Initialize(IInputControl inputControl)
    {
        inputControl.SetValue(getter());
        inputControl.SetPlaceholder(displayName);

        Task? waitTask = null;
        var cts = new CancellationTokenSource();

        inputControl.ValueChangedCommand = new Command(async value =>
        {
            // only execute/show the validation after 800ms without input changes.
            setter(value);

            if (waitTask is not null)
            {
                cts.Cancel();
                cts.Dispose();
                cts = new CancellationTokenSource();
            }

            waitTask = Task.Delay(800, cts.Token);

            await waitTask
                .ContinueWith(t =>
                {
                    if (t.IsCanceled) return;

                    _ = form.IsValid(propertyName);
                    var error = form.GetError(propertyName);

                    inputControl.Dispatch(() => inputControl.ValidationMessage = error);
                });
        });

        form.OnFormValidated += f =>
            inputControl.ValidationMessage = f.GetError(propertyName);
        form.OnModelChanged += f =>
            Initialize(inputControl);
    }

    /// <summary>
    /// Updates the view, this method is useful to update the control when the model changed in code.
    /// </summary>
    /// <param name="inputControl">The control to update.</param>
    public void UpdateView(IInputControl inputControl)
    {
        inputControl.SetValue(getter());
    }
}
