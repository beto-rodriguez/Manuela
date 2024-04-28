namespace Manuela.Forms;

public class Form
{
    public static Func<Form, string?, bool> Validator { get; } = Validators.DataAnnotationsValidator;
    public Dictionary<string, string> Errors { get; } = [];

    public event Action<Form>? OnFormValidated;

    public virtual bool IsValid(string? property = null)
    {
        var isValid = Validator(this, property);

        // if all the properties were valuated (prroperty == null)
        // then rraise the event.
        if (property is null) OnFormValidated?.Invoke(this);

        return isValid;
    }

    public string GetError(string propertyName)
    {
        return Errors.TryGetValue(propertyName, out var message)
            ? message
            : string.Empty;
    }

    public virtual object? GetModel() => null;
}

public class Form<T>() : Form
    where T : new()
{
    public T Model { get; set; } = new();

    public override object? GetModel() => Model;

    protected virtual void OnInitialized()
    { }
}
