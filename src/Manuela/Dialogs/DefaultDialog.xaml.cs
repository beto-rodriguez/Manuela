using Manuela.Forms;

namespace Manuela.Dialogs;

public partial class DefaultDialog : VerticalStackLayout
{
    public DefaultDialog()
    {
        InitializeComponent();
    }

    public void SetContent(
        string? title,
        string? message,
        ModalOptions answerType = ModalOptions.Ok)
    {
        titleLabel.Text = title;
        messageLabel.Text = message;

        okBtn.IsVisible = answerType.HasFlag(ModalOptions.Ok);
        yesBtn.IsVisible = answerType.HasFlag(ModalOptions.Yes);
        noBtn.IsVisible = answerType.HasFlag(ModalOptions.No);
        cancelBtn.IsVisible = answerType.HasFlag(ModalOptions.Cancel);
    }

    private void cancelBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(ModalOptions.Cancel);
    private void noBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(ModalOptions.No);
    private void yesBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(ModalOptions.Yes);
    private void okBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(ModalOptions.Ok);

}
