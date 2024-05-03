using System.ComponentModel.DataAnnotations;

namespace Gallery.Views.ValidationObjects;

public class Customer
{
    [Display(Name = "Please inster a name")]
    [Required]
    [MinLength(10)]
    public string? Name { get; set; } = "juan";

    [EmailAddress]
    public string? Email { get; set; } = "not a mail";

    [DateYearsRange(-100, 100)]
    public DateTime? BirthDate { get; set; } = DateTime.Now.AddYears(-200);

    [Required]
    public string? Country { get; set; }

    [MinLength(50)]
    public string? Description { get; set; } = "This is a short description";

    [Display(Name = "Subcribe to newsletter?")]
    [MustBeTrue(ErrorMessage = "Please subcribe to continue")]
    public bool IsSubscribed { get; set; }
}

public class DateYearsRangeAttribute(int minOffset, int maxOffset)
    : RangeAttribute(
        typeof(DateTime),
        DateTime.Now.AddYears(minOffset).ToShortDateString(),
        DateTime.Now.AddYears(maxOffset).ToShortDateString())
{

}

public class MustBeTrueAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is not null && (bool)value;
    }
}
