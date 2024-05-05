namespace Gallery.Views;

public partial class Dialogs : ContentView
{
    public Dialogs()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        AppLayout.Current.ShowModal(null);
    }
}
