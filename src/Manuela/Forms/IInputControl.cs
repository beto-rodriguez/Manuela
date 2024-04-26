using System.Windows.Input;

namespace Manuela.Forms;

public interface IInputControl
{
    PropertyInput For { get; set; }
    ICommand ValueChangedCommand { get; set; }
    string ValidationMessage { get; set; }
    void SetValue(object? value);
    void SetPlaceholder(string placeholder);
}
