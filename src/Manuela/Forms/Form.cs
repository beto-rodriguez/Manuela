namespace Manuela.Forms;

public abstract class Form
{
    public static Func<Form, string?, bool> Validator { get; } = Validators.DataAnnotationsValidator;

    public Dictionary<string, string> Errors { get; } = [];
    public abstract bool IsValid(string? property = null);
    public abstract object? GetModel();
}

public class Form<T>() : Form
    where T : new()
{
    public T Model { get; set; } = new();

    public override bool IsValid(string? property = null)
    {
        return Validator(this, property);
    }

    public override object? GetModel() => Model;

    protected virtual void OnInitialized()
    { }
}
