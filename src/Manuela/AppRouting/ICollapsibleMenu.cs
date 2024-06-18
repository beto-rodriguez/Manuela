namespace Manuela.AppRouting;

public interface ICollapsibleMenu
{
    public bool IsOpen { get; }
    void Open(bool animated = true);
    void Close(bool animated = true);
    void Toggle(bool animated = true);
}
