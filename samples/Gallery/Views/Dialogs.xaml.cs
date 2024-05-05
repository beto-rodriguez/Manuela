using Gallery.Views.CustomDialogs;
using Manuela.Dialogs;

namespace Gallery.Views;

public partial class Dialogs : ContentView
{
    public Dialogs()
    {
        InitializeComponent();
    }

    private async void ShowDefaultDialog(object sender, EventArgs e)
    {
        var answer = await Modal.Show(
            "Select an option",
            "Modals can be awaited to know the user result, please select an option:",
            ModalOptions.YesNoCancel);

        var message = $"you picked {answer}";
    }

    private void ShowSmallDialog(object sender, EventArgs e)
    {
        Modal.Show("Small", $"This is small", ModalOptions.Ok, DialogSize.Small);
    }

    private void ShowMediumDialog(object sender, EventArgs e)
    {
        Modal.Show("Medium", $"This is medium", ModalOptions.Ok, DialogSize.Medium);
    }

    private void ShowLargeDialog(object sender, EventArgs e)
    {
        Modal.Show("Large", $"This is large", ModalOptions.Ok, DialogSize.Large);
    }

    private async void ShowCustomDialog(object sender, EventArgs e)
    {
        var answer = await Modal.Show<PickerItem>(new MyModalPicker());

        var message = $"you picked {answer?.Name}";
    }

    private void ShowNestedDialog(object sender, EventArgs e)
    {
        Modal.Show<string>(new Nested());
    }
}
