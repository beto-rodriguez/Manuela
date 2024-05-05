using Manuela.Dialogs;

namespace Gallery.Views.CustomDialogs;

public partial class Nested : ContentView
{
    public Nested()
    {
        InitializeComponent();
    }

    private async void ShowNested(object sender, EventArgs e)
    {
        Modal.Show<string>(new Nested());
    }

    private void Close(object sender, EventArgs e)
    {
        // CancelDialog is an extension method that cancels the dialog TaskCompletionSource
        this.CancelDialog();
    }
}
