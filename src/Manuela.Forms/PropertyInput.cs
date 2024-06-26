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

        Timer? waitTimer = null;
        inputControl.InputValueChangedCommand = new Command(value =>
        {
            // only execute/show the validation after 800ms without input changes.
            setter(value);

            waitTimer?.Dispose();
            waitTimer = new(state =>
            {
                _ = form.IsValid(propertyName);
                var error = form.GetError(propertyName);

                inputControl.Dispatch(() => inputControl.ValidationMessage = error);
            }, new(), 800, Timeout.Infinite);
        });

        form.OnFormValidated += f => inputControl.Dispatch(() => inputControl.ValidationMessage = f.GetError(propertyName));
        form.OnModelChanged += f =>
        {
            inputControl.SetValue(getter());
            inputControl.SetPlaceholder(displayName);
        };
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
