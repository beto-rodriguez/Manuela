// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States;

namespace Manuela;

public class Selected : ConditionalStyle
{
    public Selected()
    {
        Condition = new(visualElement => false);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        // could more states use Maui VisualStates?
        // yes... but I really prefer Manuela implementation, it just subscribes to the property changed event.
        // but in the case of the "Selected" state, there is no Bindable/Attached property to subscribe to.
        // Maybe Maui should add an attached property for this case???

        var selectedState = new VisualState { Name = "Selected" };
        foreach (var item in GetSetters()?.AsMauiSetters(visualElement) ?? [])
            selectedState.Setters.Add(item);

        visualElement.SetValue(
            VisualStateManager.VisualStateGroupsProperty,
            new VisualStateGroupList
            {
                new VisualStateGroup
                {
                    Name = "CommonStates",
                    States =
                    {
                        new VisualState { Name = "Normal" },
                        selectedState
                    }
                }
            });

        Condition.Triggers = [new(visualElement, [])];
    }
}
