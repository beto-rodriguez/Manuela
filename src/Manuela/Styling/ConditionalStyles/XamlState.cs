namespace Manuela.Styling.ConditionalStyles;

/// <summary>
/// A class that represents a Manuela conditional style, objects that inherit from this class, will be target for code generation,
/// Manuela will generate the necessary code, and subcribe to the INotifyPropertyChanged events to apply the style when required.
/// </summary>
public abstract class XamlState : ConditionalStyle
{
    public abstract bool IsActive(VisualElement visualElement);
}
