using Manuela.Dialogs;

namespace Gallery.Views;

public partial class Dialogs : ContentView
{
    public Dialogs()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var ans = await Modal.Show(
            "title here",
            "hi this is a really long messagehi this is a really long messagehi this ", Answer.YesNo);

        var a = 1;
    }
}
