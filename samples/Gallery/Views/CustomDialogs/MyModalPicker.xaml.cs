using Manuela.Dialogs;

namespace Gallery.Views.CustomDialogs;

public partial class MyModalPicker : ContentView
{
    public MyModalPicker()
    {
        InitializeComponent();
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        this.SetDialogResponse(e.CurrentSelection.FirstOrDefault());
    }
}
