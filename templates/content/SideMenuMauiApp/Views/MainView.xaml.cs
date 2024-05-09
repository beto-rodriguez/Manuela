using Manuela.Dialogs;

namespace SideMenuMauiApp.Views;

public partial class MainView : ContentView
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var answer = await Modal.Show(
            "Select an option",
            "Modals can be awaited to know the user result, please select an option:",
            ModalOptions.YesNoCancel);

        var message = $"you picked {answer}";
    }
}
