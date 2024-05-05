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
        Answer answerType = Answer.Ok)
    {
        titleLabel.Text = title;
        messageLabel.Text = message;

        okBtn.IsVisible = answerType.HasFlag(Answer.Ok);
        yesBtn.IsVisible = answerType.HasFlag(Answer.Yes);
        noBtn.IsVisible = answerType.HasFlag(Answer.No);
        cancelBtn.IsVisible = answerType.HasFlag(Answer.Cancel);
    }

    private void cancelBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(Answer.Cancel);
    private void noBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(Answer.No);
    private void yesBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(Answer.Yes);
    private void okBtn_Clicked(object sender, EventArgs e) => this.SetDialogResponse(Answer.Ok);

}
