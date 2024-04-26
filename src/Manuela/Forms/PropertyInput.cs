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

        inputControl.ValueChangedCommand = new Command(value =>
        {
            setter(value);

            _ = form.IsValid(propertyName);

            inputControl.ValidationMessage =
                form.Errors.TryGetValue(propertyName, out var message)
                ? message
                : string.Empty;
        });
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
