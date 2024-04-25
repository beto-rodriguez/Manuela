using System.ComponentModel.DataAnnotations;

namespace Manuela.Forms;

public abstract class Form
{
    public Dictionary<string, string> Errors { get; } = [];
    public abstract bool IsValid(string? property = null);
}

public class Form<T>() : Form
    where T : new()
{
    public T Model { get; set; } = new();

    public override bool IsValid(string? property = null)
    {
        if (Model is null) return true;

        if (property is null)
            Errors.Clear();
        else
            _ = Errors.Remove(property);

        var context = new ValidationContext(Model);
        var results = new List<ValidationResult>();
        _ = Validator.TryValidateObject(Model, context, results, true);

        var isValid = true;

        foreach (var result in results)
            foreach (var member in result.MemberNames)
            {
                if (property is not null && member != property) continue;

                AddValidationError(
                    member,
                    result.ErrorMessage ?? "Unknown error.",
                    ref isValid);
            }

        return isValid;
    }

    protected void AddValidationError(string propertyName, string errorMessage, ref bool isValid)
    {
        // if there is already an error for this member, skip it
        if (Errors.ContainsKey(propertyName)) return;

        Errors[propertyName] = errorMessage;
        isValid = false;
    }

    protected virtual void OnInitialized()
    { }
}
