using Manuela.Forms;

namespace Gallery.Views.ValidationObjects;

public partial class CustomerForm : Form<Customer>
{
    public Command SaveCommand => new(() =>
    {
        if (!IsValid())
        {
            // the Errors property is a dictionary with
            // the property name as the key and
            // the error message as the value.
            var errorsDictionary = Errors;

            // do something with the errors? maybe show them to the user?

            return;
        }

        // the model is valid at this point.
        // save the record maybe?
        var customer = Model;
    });
}
