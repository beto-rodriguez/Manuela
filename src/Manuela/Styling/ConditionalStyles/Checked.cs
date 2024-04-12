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
        Condition = new(ConditionDefinition);
    }

    public override void Dispose(VisualElement visualElement)
    {
        if (_element is not null)
            _element.PropertyChanged -= _propertyChangedEventHandler;

        _element = null;

        base.Dispose(visualElement);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        _element = visualElement;

        if (visualElement is CheckBox checkBox)
        {
            _propertyChangedEventHandler += (sender, e) =>
            {
                if (e.PropertyName is null or not (nameof(CheckBox.IsChecked)))
                    return;

                visualElement.SetValue(Has.IsCheckedStateProperty, checkBox.IsChecked);
            };

            checkBox.PropertyChanged += _propertyChangedEventHandler;
        }

        if (visualElement is RadioButton radioButton)
        {
            _propertyChangedEventHandler = (sender, e) =>
            {
                if (e.PropertyName is null or not (nameof(RadioButton.IsChecked)))
                    return;

                visualElement.SetValue(Has.IsCheckedStateProperty, radioButton.IsChecked);
            };

            radioButton.PropertyChanged += _propertyChangedEventHandler;
        }

        Condition.Triggers = [new(visualElement, [Has.IsCheckedStateProperty.PropertyName])];
    }

    protected virtual bool ConditionDefinition(VisualElement visualElement)
    {
        return (bool)visualElement.GetValue(Has.IsCheckedStateProperty);
    }
}
