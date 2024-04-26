using System.ComponentModel.DataAnnotations;
using Manuela.Forms;

namespace Gallery.Views;

public partial class Validation : ContentView
{
    public Validation()
    {
        InitializeComponent();

        BindingContext = new CustomerForm();
    }
}

public class Customer
{
    [Display(Name = "Please inster a name")]
    [Required]
    [MinLength(10)]
    public string? Name { get; set; } = "juan peres";

    [EmailAddress]
    public string? Email { get; set; }
}

public partial class CustomerForm : Form<Customer>
{
    public Command SaveCommand => new(() =>
    {
        var v = Model.Name;
        if (IsValid())
        {
            // save record here
        }
    });
}
