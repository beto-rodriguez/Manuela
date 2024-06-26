using System.ComponentModel;

namespace Manuela.Forms;

public abstract class Form : INotifyPropertyChanged
{
    private object? _model;
    private bool _isEnabled = true;

    public static Func<Form, string?, bool> Validator { get; } = Validators.DataAnnotationsValidator;
    public Dictionary<string, string> Errors { get; } = [];

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsDisabled));
        }
    }

    public bool IsDisabled => !IsEnabled;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<Form>? OnFormValidated;
    public event Action<Form>? OnModelChanged;

    public virtual bool IsValid(string? property = null)
    {
        var isValid = Validator(this, property);

        // if all the properties were evaluated (property == null)
        // then raise the event.
        if (property is null) OnFormValidated?.Invoke(this);

        return isValid;
    }

    public string GetError(string propertyName)
    {
        return Errors.TryGetValue(propertyName, out var message)
            ? message
            : string.Empty;
    }

    public void SetErrors(IEnumerable<ValidationError> errors)
    {
        SetErrors(errors.ToArray());
    }

    public void SetErrors(params ValidationError[] errors)
    {
        foreach (var error in errors)
            Errors[error.PropertyName] = error.Message;

        OnFormValidated?.Invoke(this);
    }

    public virtual object? GetModel()
    {
        return _model;
    }

    public virtual void SetModel(object? model)
    {
        _model = model;
        InvokeModelChanged();
    }

    protected virtual void InvokeModelChanged()
    {
        OnModelChanged?.Invoke(this);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class Form<T> : Form
    where T : new()
{
    public Form()
    {
        SetModel(new T());
    }

    public T Model => (T)GetModel()!;

    protected virtual void OnInitialized()
    { }
}
