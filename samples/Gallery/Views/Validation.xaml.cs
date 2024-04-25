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
    [Display(Name = "Name")]
    [Required]
    [MinLength(10)]
    public string? Name { get; set; } = "juan peres";

    [EmailAddress]
    public string? Email { get; set; }
}

public class CustomerForm : Form<Customer>
{
    public PropertyInput NameInput { get; }

    public CustomerForm()
    {
        OnInitialized();

        NameInput = new(
            this,
            "Name",
            "Please inset a name",
            getter: () => Model.Name,
            setter: value =>
            {
                var str = (string?)value;
                if (str?.Length == 0) str = null;
                Model.Name = str;
            });
    }

    protected override void OnInitialized()
    {
        Model = new Customer();
    }

    public Command SaveCommand => new(() =>
    {
        var v = Model.Name;
        if (IsValid())
        {
            // save record here
        }
    });
}
