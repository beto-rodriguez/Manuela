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
        // when a view is displayed in a modal, a TaskCompletionSource is attached to the view.
        // you can use the SetDialogResponse extensioon method to set the result of the assigned TCS.

        this.SetDialogResponse(e.CurrentSelection.FirstOrDefault());
    }
}
