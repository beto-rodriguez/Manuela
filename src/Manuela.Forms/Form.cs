using System.ComponentModel;

namespace Manuela.Forms;

public class Form
{
    public static Func<Form, string?, bool> Validator { get; } = Validators.DataAnnotationsValidator;
    public Dictionary<string, string> Errors { get; } = [];

    public event Action<Form>? OnFormValidated;
    public event Action<Form>? OnModelChanged;

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

    public virtual object? GetModel()
    {
        return null;
    }

    protected virtual void InvokeModelChanged()
    {
        OnModelChanged?.Invoke(this);
    }
}

public class Form<T>() : Form, INotifyPropertyChanged
    where T : new()
{
    private T _model = new();

    public T Model
    {
        get => _model;
        set
        {
            _model = value;
            OnPropertyChanged(nameof(Model));
            InvokeModelChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public override object? GetModel()
    {
        return Model;
    }

    protected virtual void OnInitialized()
    { }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
