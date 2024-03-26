namespace Manuela.Styling.ConditionalStyles;

public class Checked : ConditionalStyle
{
    public Checked()
    {
        Condition = new(ConditionDefinition)
        {
            Triggers = v =>
            {
                if (v is CheckBox checkBox)
                {
                    checkBox.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(CheckBox.IsChecked)))
                            return;

                        v.SetValue(Has.IsCheckedStateProperty, checkBox.IsChecked);
                    };

                    return [new(v, [nameof(CheckBox.IsChecked)])];
                }

                if (v is RadioButton radioButton)
                {
                    radioButton.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName is null or not (nameof(RadioButton.IsChecked)))
                            return;

                        v.SetValue(Has.IsCheckedStateProperty, radioButton.IsChecked);
                    };

                    return [new(v, [nameof(RadioButton.IsChecked)])];
                }

#if DEBUG
                throw new Exception(
                    $"{nameof(Checked)} trigger is not supported in elements of type {v.GetType()}.");
#endif

            }
        };
    }

    protected virtual bool ConditionDefinition(VisualElement visualElement)
    {
        return (bool)visualElement.GetValue(Has.IsCheckedStateProperty);
    }
}
