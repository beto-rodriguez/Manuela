using System.Windows.Input;

namespace Manuela.Forms;

public interface IInputControl
{
    PropertyInput InputOf { get; set; }
    string ValidationMessage { get; set; }
    void SetValue(object? value);
    void SetPlaceholder(string placeholder);
    ICommand ValueChangedCommand { get; set; }
}
