using System.ComponentModel.DataAnnotations;

namespace Manuela.Forms;

public static class Validators
{
    public static Func<Form, string?, bool> DataAnnotationsValidator { get; }
        = (form, propertyName) =>
        {
            var model = form.GetModel();

            if (model is null) return true;

            if (propertyName is null)
                form.Errors.Clear();
            else
                _ = form.Errors.Remove(propertyName);

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            _ = Validator.TryValidateObject(model, context, results, true);

            var isValid = true;

            foreach (var result in results)
                foreach (var member in result.MemberNames)
                {
                    if (propertyName is not null && member != propertyName) continue;

                    // Skip if the property already has an error.
                    if (form.Errors.ContainsKey(member)) continue;

                    form.Errors[member] = result.ErrorMessage ?? "Unknown error.";
                    isValid = false;
                }

            return isValid;
        };
}
