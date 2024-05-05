namespace Manuela.Dialogs;

[Flags]
public enum Answer
{
    Ok = 1 << 0,
    Yes = 1 << 1,
    No = 1 << 2,
    Cancel = 1 << 3,
    YesNo = Yes | No,
    OkCancel = Ok | Cancel,
    YesNoCancel = Yes | No | Cancel
}
