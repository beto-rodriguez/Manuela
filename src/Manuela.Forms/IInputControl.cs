using System.Windows.Input;

namespace Manuela.Forms;

public interface IInputControl
{
    PropertyInput For { get; set; }
    bool IsEnabled { get; set; }
    ICommand InputValueChangedCommand { get; set; }
    string ValidationMessage { get; set; }
    void SetValue(object? value);
    void SetPlaceholder(string placeholder);
    void Dispatch(Action action);
}
