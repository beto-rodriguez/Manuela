// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.ComponentModel;
using Manuela.Styling.ConditionalStyles;

namespace Manuela;

public class Checked : ConditionalStyle
{
    private VisualElement? _element;
    private PropertyChangedEventHandler? _propertyChangedEventHandler;

    public Checked()
    {
        Condition = new(ConditionDefinition)
        {
            Triggers = v =>
            {
                _element = v;

                if (v is CheckBox checkBox)
                {
                    _propertyChangedEventHandler += (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(CheckBox.IsChecked)))
                            return;

                        v.SetValue(Has.IsCheckedStateProperty, checkBox.IsChecked);
                    };

                    checkBox.PropertyChanged += _propertyChangedEventHandler;

                    return [new(v, [nameof(CheckBox.IsChecked)])];
                }

                if (v is RadioButton radioButton)
                {
                    _propertyChangedEventHandler = (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(RadioButton.IsChecked)))
                            return;

                        v.SetValue(Has.IsCheckedStateProperty, radioButton.IsChecked);
                    };

                    radioButton.PropertyChanged += _propertyChangedEventHandler;

                    return [new(v, [nameof(RadioButton.IsChecked)])];
                }

                throw new Exception(
                    $"{nameof(Checked)} trigger is not supported in elements of type {v.GetType()}.");
            }
        };
    }

    public override void Dispose()
    {
        if (_element is not null)
            _element.PropertyChanged -= _propertyChangedEventHandler;

        _element = null;

        base.Dispose();
    }

    protected virtual bool ConditionDefinition(VisualElement visualElement)
    {
        return (bool)visualElement.GetValue(Has.IsCheckedStateProperty);
    }
}
