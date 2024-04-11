using Manuela.Styling;

namespace Gallery.Views;

public partial class States : ContentView
{
    public States()
    {
        InitializeComponent();
    }

    private void ToggleState(object sender, TappedEventArgs e)
    {
        var button = (Button)sender;

        var state = button.GetCustomState();

        if (state == "active")
        {
            button.SetCustomState(null);
        }
        else
        {
            button.SetCustomState("active");
        }
    }
}
