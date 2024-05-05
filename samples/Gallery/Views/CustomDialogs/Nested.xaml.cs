using Manuela.Dialogs;

namespace Gallery.Views.CustomDialogs;

public partial class Nested : ContentView
{
    public Nested()
    {
        InitializeComponent();
    }

    private void ShowNested(object sender, EventArgs e)
    {
        _ = Modal.Show<bool>(new Nested());
    }

    private void Close(object sender, EventArgs e)
    {
        this.CancelDialog();
    }
}
